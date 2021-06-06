// fw_app.h
// Firmware application.

#ifndef FW_APP_H
#define FW_APP_H

#include "main.h"
#include "usart.h"
#include "gpio.h"

#include "fl_def.h"
#include "fl_queue.h"
#include "fl_message_def.h"
#include "fl_stm32.h"
#include "fl_util.h"
#include "fl_i2c.h"

// Parser defines
#define FW_APP_TXT_PARSER           (0)

#define FW_APP_PARSER               FW_APP_TXT_PARSER

#define FW_APP_PARSER_CALLBACK      (1) // 0 : No parser callback, 1 : Parser callback

#include "fl_txt_message.h"
#include "fl_txt_message_parser.h"

#define FW_APP_HW_MAJOR             (0)
#define FW_APP_HW_MINOR             (0)
#define FW_APP_HW_REVISION          (1)

#define FW_APP_FW_MAJOR             (0)
#define FW_APP_FW_MINOR             (2)
#define FW_APP_FW_REVISION          (1)


#define FW_APP_UART_HANDLE                                UART_HandleTypeDef*
#define FW_APP_GPIO_HANDLE                                GPIO_TypeDef*
#define FW_APP_GPIO_TOGGLE(pin, port)                     HAL_GPIO_TogglePin(port, pin)
#define FW_APP_UART_RCV_IT(handle, buf, count)            HAL_UART_Receive_IT(handle, buf, count)
#define FW_APP_UART_TRANSMIT(handle, buf, count, timeout) HAL_GPIO_Transmit(handle, buf, count, timeout)

#define FW_APP_DEBUG_PACKET_LENGTH  (128)

#define FW_APP_ONE_SEC_INTERVAL     (999) // 1 second

#define FW_APP_PROTO_TX_TIMEOUT     (500)

FL_BEGIN_PACK1

typedef struct _fw_app_debug_manager
{
  FW_APP_UART_HANDLE    uart_handle;
  uint8_t               buf[FW_APP_DEBUG_PACKET_LENGTH];
  uint8_t               length;
} fw_app_debug_manager_t;

// Protocol manager
typedef struct _fw_app_proto_manager
{
  // UART handle.
  FW_APP_UART_HANDLE    uart_handle;

  // Buffer for received bytes.
  fl_queue_t            q;
  fl_txt_msg_parser_t   parser_handle;
  uint8_t               out_buf[FL_TXT_MSG_MAX_LENGTH];
  uint8_t               out_length;
  uint8_t               rx_buf[1];
} fw_app_proto_manager_t;


// Firmware application manager.
typedef struct _fw_app
{
  uint32_t                device_id;
  // Current tick count.
  volatile uint32_t       tick;

  // Protocol manager.
  fw_app_proto_manager_t  proto_mgr;
  fl_i2c_t                i2c;
} fw_app_t;

FL_END_PACK

FL_BEGIN_DECLS

FL_DECLARE_DATA extern fw_app_t g_app;

FL_DECLARE(void) fw_app_init(void);
FL_DECLARE(void) fw_app_hw_init(void);
FL_DECLARE(void) fw_app_systick(void);
FL_END_DECLS

#endif
