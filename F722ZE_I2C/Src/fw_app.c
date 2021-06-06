#include <stdio.h>
#include <string.h>
#include "i2c.h"
#include "fw_app.h"

typedef struct _vl6180x_addr_info
{
  uint16_t  addr;
  uint16_t  reg_bytes;
} vl6180x_addr_info_t;

extern TIM_HandleTypeDef htim2;

FL_DECLARE_DATA fw_app_t g_app;

vl6180x_addr_info_t      g_addrs[] = {
    {0x0000, 1},
    {0x002E, 1},
    {0x0031, 1},
    {0x0038, 1},
    {0x003A, 2},
    {0x003C, 2},
    {0x003E, 1},
    {0x003F, 1},
    {0x0040, 2},
    {0x004D, 1},
    {0x004E, 1},
    {0x004F, 1},
    {0x0050, 2},
    {0x0062, 1},
    {0x0066, 2},
    {0x0068, 2},
    {0x006C, 4},
    {0x0070, 4},
    {0x0074, 4},
    {0x0078, 4},
    {0x007C, 4},
    {0x010A, 1},
    {0x0119, 1},
    {0x0120, 1},
    {0x002D, 1},
    {0x0212, 1},
    {0x002C, 1},
    {0x0025, 1},
    {0x0001, 1},
    {0x0002, 1},
    {0x0003, 1},
    {0x0004, 1},
    {0x0006, 1},
    {0x0007, 1},
    {0x0008, 2},
    {0x0010, 1},
    {0x0011, 1},
    {0x0012, 1},
    {0x0014, 1},
    {0x0015, 1},
    {0x0016, 1},
    {0x0017, 1},
    {0x0018, 1},
    {0x0019, 1},
    {0x001A, 1},
    {0x001B, 1},
    {0x001C, 1},
    {0x001E, 2},
    {0x0021, 1},
    {0x0022, 2},
    {0x0024, 1},
    {0x0026, 2},
    {0x02A3, 1}
};

#if FW_APP_PARSER_CALLBACK == 1
static void on_message_parsed(const void* parser_handle, void* context);
#endif

static uint16_t vl6180x_reg_byte_count(uint16_t reg_addr);

FL_DECLARE(void) fw_app_init(void)
{
  memset(&g_app, 0, sizeof(g_app));

  // Serial port for message communication.
  g_app.proto_mgr.uart_handle = &huart3;
  g_app.proto_mgr.parser_handle.on_parsed_callback = on_message_parsed;
  g_app.proto_mgr.parser_handle.context = (void*)&g_app;

  g_app.i2c.i2c = &hi2c1;
  g_app.i2c.timeout = 1000;
}

FL_DECLARE(void) fw_app_hw_init(void)
{
  // TODO : Device id setting(DIP switch, flash storage, ...).
  g_app.device_id = 1;

  // GPIO output pin for debugging.
  HAL_GPIO_WritePin(DBG_OUT1_GPIO_Port, DBG_OUT1_Pin, GPIO_PIN_RESET);
  HAL_GPIO_WritePin(DBG_OUT2_GPIO_Port, DBG_OUT2_Pin, GPIO_PIN_RESET);

  // Message receive in interrupt mode.
  FW_APP_UART_RCV_IT(g_app.proto_mgr.uart_handle, g_app.proto_mgr.rx_buf, 1);
}

FL_DECLARE(void) fw_app_systick(void)
{
  g_app.tick++;
  // Do some work every 1 second.
  if (g_app.tick >= FW_APP_ONE_SEC_INTERVAL)
  {
    // LED1 toggle.
    HAL_GPIO_TogglePin(LD1_GPIO_Port, LD1_Pin);
    g_app.tick = 0;
  }
}

#if FW_APP_PARSER_CALLBACK == 1
static void on_message_parsed(const void* parser_handle, void* context)
{
  fl_txt_msg_parser_t*    txt_parser = (fl_txt_msg_parser_t*)parser_handle;
  fw_app_proto_manager_t* proto_mgr = &((fw_app_t*)context)->proto_mgr;
//  fl_bool_t           cmd_processed = FL_FALSE;
  fl_status_t         ret = FL_ERROR;

  // Ignore the parsed message.
  if (txt_parser->device_id != ((fw_app_t*)context)->device_id)
  {
    return;
  }

  switch (txt_parser->msg_id)
  {
  case FL_MSG_ID_READ_HW_VERSION:
  {
    proto_mgr->out_length = sprintf((char*)proto_mgr->out_buf, "%s %ld,%d,%d.%d.%d%c",
        fl_txt_msg_get_message_name(txt_parser->msg_id),
        txt_parser->device_id,
        FL_OK,
        FW_APP_HW_MAJOR, FW_APP_HW_MINOR, FW_APP_HW_REVISION,
        FL_TXT_MSG_TAIL);
    break;
  }

  case FL_MSG_ID_READ_FW_VERSION:
  {
    proto_mgr->out_length = sprintf((char*)proto_mgr->out_buf, "%s %ld,%d,%d.%d.%d%c",
        fl_txt_msg_get_message_name(txt_parser->msg_id),
        txt_parser->device_id,
        FL_OK,
        FW_APP_FW_MAJOR, FW_APP_FW_MINOR, FW_APP_FW_REVISION,
        FL_TXT_MSG_TAIL);
    break;
  }

  case FL_MSG_ID_READ_WRITE_I2C:
  {
    // I2C read
    if (txt_parser->arg_count == 4)
    {
      fl_i2c_read_t* i2c_read = (fl_i2c_read_t*)&(proto_mgr->parser_handle.payload);
      uint32_t data = 0;
      uint16_t byte_count = vl6180x_reg_byte_count(i2c_read->reg_addr);

      if (byte_count == 1)
      {
        ret = fl_i2c_read_byte(&g_app.i2c, i2c_read->dev_addr, i2c_read->reg_addr, (uint8_t*)&data);
      }
      else if (byte_count == 2)
      {
        ret = fl_i2c_read_word(&g_app.i2c, i2c_read->dev_addr, i2c_read->reg_addr, (uint16_t*)&data);
      }
      else if (byte_count == 4)
      {
        ret = fl_i2c_read_dword(&g_app.i2c, i2c_read->dev_addr, i2c_read->reg_addr, &data);
      }

      if (ret == FL_OK)
      {
        proto_mgr->out_length = sprintf((char*)proto_mgr->out_buf, "%s %ld,%d,%d,%d,%d,%d,%ld%c",
                fl_txt_msg_get_message_name(txt_parser->msg_id),
                txt_parser->device_id,
                FL_OK,
                i2c_read->rw_mode,
                i2c_read->i2c_num,
                i2c_read->dev_addr,
                i2c_read->reg_addr,
                data,
                FL_TXT_MSG_TAIL);
      }
    }
    // I2C write
    else if (txt_parser->arg_count == 5)
    {
      fl_i2c_write_t* i2c_wr = (fl_i2c_write_t*)&(proto_mgr->parser_handle.payload);
      uint16_t byte_count = vl6180x_reg_byte_count(i2c_wr->reg_addr);

      if (byte_count == 1)
      {
        ret = fl_i2c_write_byte(&g_app.i2c, i2c_wr->dev_addr, i2c_wr->reg_addr, (uint8_t)i2c_wr->reg_value);
      }
      else if (byte_count == 2)
      {
        ret = fl_i2c_write_word(&g_app.i2c, i2c_wr->dev_addr, i2c_wr->reg_addr, (uint16_t)i2c_wr->reg_value);
      }
      else if (byte_count == 4)
      {
        ret = fl_i2c_write_dword(&g_app.i2c, i2c_wr->dev_addr, i2c_wr->reg_addr, i2c_wr->reg_value);
      }

      if (ret == FL_OK)
      {
        proto_mgr->out_length = sprintf((char*)proto_mgr->out_buf, "%s %ld,%d%c",
                          fl_txt_msg_get_message_name(txt_parser->msg_id),
                          txt_parser->device_id,
                          FL_OK,
                          FL_TXT_MSG_TAIL);
      }
    }

    if (proto_mgr->out_length == 0)
    {
      proto_mgr->out_length = sprintf((char*)proto_mgr->out_buf, "%s %ld,%d%c",
                fl_txt_msg_get_message_name(txt_parser->msg_id),
                txt_parser->device_id,
                FL_ERROR,
                FL_TXT_MSG_TAIL);
    }
    break;
  }
  }

  if (proto_mgr->out_length > 0)
  {
    HAL_UART_Transmit(proto_mgr->uart_handle, proto_mgr->out_buf, proto_mgr->out_length, FW_APP_PROTO_TX_TIMEOUT);
  }
  proto_mgr->out_length = 0;
}
#endif

static uint16_t vl6180x_reg_byte_count(uint16_t reg_addr)
{
  int i = 0;

  for (i = 0; i < sizeof(g_addrs)/sizeof(g_addrs[0]); i++)
  {
    if (g_addrs[i].addr == reg_addr)
    {
      return g_addrs[i].reg_bytes;
    }
  }

  return 0;
}
