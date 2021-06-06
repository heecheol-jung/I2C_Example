#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include "fl_txt_message.h"

FL_DECLARE(uint8_t) fl_txt_msg_build_command(
  const uint32_t device_id,
  const uint8_t message_id,
  void* arg_buf,
  uint32_t arg_buf_len,
  uint8_t* packet_buf,
  uint32_t packet_buf_len)
{
  uint32_t len = 0;

  if ((packet_buf == NULL) ||
      (packet_buf_len == 0))
  {
    return len;
  }

  if (arg_buf != NULL)
  {
    if (arg_buf_len == 0)
    {
      return len;
    }
    else if (arg_buf_len > sizeof(fl_hw_ver_t))
    {
      return len;
    }
  }
  else
  {
    if (arg_buf != 0)
    {
      return len;
    }
  }

  switch (message_id)
  {
  case FL_MSG_ID_READ_HW_VERSION:
  {
    // RHVER device_id\n
    // ex) RHVER 1\n
    len = sprintf((char*)packet_buf, "%s %ld%c", fl_txt_msg_get_message_name(message_id), device_id, FL_TXT_MSG_TAIL);
    break;
  }

  case FL_MSG_ID_READ_FW_VERSION:
  {
    // RFVER device_id\n
    // ex) RFVER 1\n
    len = sprintf((char*)packet_buf, "%s %ld%c", fl_txt_msg_get_message_name(message_id), device_id, FL_TXT_MSG_TAIL);
    break;
  }
  }

  if (len > packet_buf_len)
  {
    len = 0;
  }

  return len;
}

FL_DECLARE(uint8_t) fl_txt_msg_build_response(
  const uint32_t device_id,
  const uint8_t message_id,
  uint8_t error,
  void* arg_buf,
  uint32_t arg_buf_len,
  uint8_t* packet_buf,
  uint32_t packet_buf_len)
{
  uint8_t len = 0;

  if ((packet_buf == NULL) ||
      (packet_buf_len == 0))
  {
    return len;
  }

  if (arg_buf != NULL)
  {
    if (arg_buf_len == 0)
    {
      return len;
    }
    else if (arg_buf_len > sizeof(fl_i2c_read_resp_t))
    {
      return len;
    }
  }
  else
  {
    if (arg_buf != 0)
    {
      return len;
    }
  }

  if (error == FL_OK)
  {
    switch (message_id)
    {
      case FL_MSG_ID_READ_HW_VERSION:
      {
        // RHVER device_id,error,version_string\n
        // ex) RHVER 1,0,1.2.3\n
        fl_hw_ver_t* hw_ver = (fl_hw_ver_t*)arg_buf;
        if ((arg_buf_len <= sizeof(fl_hw_ver_t)) &&
            (strlen(hw_ver->version) > 0))
        {
          len = sprintf((char*)packet_buf, "%s %ld,%d,%s%c", fl_txt_msg_get_message_name(message_id), device_id, error, hw_ver->version, FL_TXT_MSG_TAIL);
        }
        break;
      }

      case FL_MSG_ID_READ_FW_VERSION:
      {
        // RFVER device_id,error,version_string\n
        // ex) RFVER 1,0,2.3.4\n
        fl_fw_ver_t* fw_ver = (fl_fw_ver_t*)arg_buf;
        if ((arg_buf_len <= sizeof(fl_fw_ver_t)) &&
          (strlen(fw_ver->version) > 0))
        {
          len = sprintf((char*)packet_buf, "%s %ld,%d,%s%c", fl_txt_msg_get_message_name(message_id), device_id, error, fw_ver->version, FL_TXT_MSG_TAIL);
        }
        break;
      }
    }
  }
  else
  {
    len = sprintf((char*)packet_buf, "%s %ld,%d%c", fl_txt_msg_get_message_name(message_id), device_id, error, FL_TXT_MSG_TAIL);
  }

  if (len > packet_buf_len)
  {
    len = 0;
  }

  return len;
}

FL_DECLARE(char*) fl_txt_msg_get_message_name(const uint8_t message_id)
{
  switch (message_id)
  {
  case FL_MSG_ID_READ_HW_VERSION:
    return FL_TXT_RHVER_STR;

  case FL_MSG_ID_READ_FW_VERSION:
    return FL_TXT_RFVER_STR;

  case FL_MSG_ID_READ_WRITE_I2C:
    return FL_TXT_RwI2C_STR;
  }

  return NULL;
}
