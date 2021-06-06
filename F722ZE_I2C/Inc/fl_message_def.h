// Firware library message
// fl_message_def.h

#ifndef FL_MESSAGE_DEF_H
#define FL_MESSAGE_DEF_H

#include "fl_def.h"

FL_BEGIN_DECLS

///////////////////////////////////////////////////////////////////////////////
// Message ID's
///////////////////////////////////////////////////////////////////////////////
// Message IDs
#define FL_MSG_ID_BASE                      (0)
#define FL_MSG_ID_UNKNOWN                   (FL_MSG_ID_BASE + 0)

// Read hardware version.
#define FL_MSG_ID_READ_HW_VERSION           (FL_MSG_ID_BASE + 1)

// Read firmware version.
#define FL_MSG_ID_READ_FW_VERSION           (FL_MSG_ID_BASE + 2)

// I2C read/write
#define FL_MSG_ID_READ_WRITE_I2C            (FL_MSG_ID_BASE + 11)

///////////////////////////////////////////////////////////////////////////////
// Defines for general messages.
///////////////////////////////////////////////////////////////////////////////
// Maximum length of argument string.
#define FL_MSG_MAX_STRING_LEN               (32)

// Device IDs
#define FL_DEVICE_ID_UNKNOWN                (0)

#define FL_DEVICE_ID_ALL                    (0xFFFFFFFF)  // Device broadcasting.

#define FL_BUTTON_RELEASED                  (0)

#define FL_BUTTON_PRESSED                   (1)

// Message type.
#define FL_MSG_TYPE_COMMAND                 (0)
#define FL_MSG_TYPE_RESPONSE                (1)
#define FL_MSG_TYPE_EVENT                   (2)
#define FL_MSG_TYPE_UNKNOWN                 (0XFF)

// Boot mode : Application
#define FL_BMODE_APP                        (0)
// Boot mode : Bootloader
#define FL_BMODE_BOOTLOADER                 (1)

#define FL_VER_STR_MAX_LEN                  (32)

#define FL_TXT_MSG_ID_MIN_CHAR              ('A')
#define FL_TXT_MSG_ID_MAX_CHAR              ('Z')
#define FL_TXT_DEVICE_ID_MIN_CHAR           ('0')
#define FL_TXT_DEVICE_ID_MAX_CHAR           ('9')
// The last character for a text message.
#define FL_TXT_MSG_TAIL                     ('\n')
// Delimiter for a message id and device id
#define FL_TXT_MSG_ID_DEVICE_ID_DELIMITER   (' ')
// Delimiter for arguments.
#define FL_TXT_MSG_ARG_DELIMITER            (',')

#define FL_MSG_I2C_MAX_PAYLOAD_LEN          (32)

#define FL_MSG_I2C_READ                     (0)
#define FL_MSG_I2C_WRITE                    (1)

FL_BEGIN_PACK1

///////////////////////////////////////////////////////////////////////////////
// structs for messages.
///////////////////////////////////////////////////////////////////////////////
typedef struct _fl_hw_ver
{
  char        version[FL_VER_STR_MAX_LEN];
} fl_hw_ver_t;

typedef struct _fl_fw_ver
{
  char        version[FL_VER_STR_MAX_LEN];
} fl_fw_ver_t;

typedef struct _fl_i2c_read
{
  uint8_t     rw_mode;      // FL_MSG_I2C_READ, FL_MSG_I2C_WRITE
  uint8_t     i2c_num;      // I2C number
  uint16_t    dev_addr;     // Target device address
  uint16_t    reg_addr;     // Register address
} fl_i2c_read_t;

typedef struct _fl_i2c_write
{
  uint8_t     rw_mode;      // FL_MSG_I2C_READ, FL_MSG_I2C_WRITE
  uint8_t     i2c_num;      // I2C number
  uint16_t    dev_addr;     // Target device address
  uint16_t    reg_addr;     // Register address
  uint32_t    reg_value;    // Register value
} fl_i2c_write_t;

typedef struct _fl_i2c_read_resp
{
  uint8_t     rw_mode;      // FL_MSG_I2C_READ, FL_MSG_I2C_WRITE
  uint8_t     i2c_num;      // I2C number
  uint16_t    dev_addr;     // Target device address
  uint16_t    reg_addr;     // Register address
  uint32_t    reg_value;    // Register value
} fl_i2c_read_resp_t;

FL_END_PACK

typedef void(*fl_msg_cb_on_parsed_t)(const void* parser_handle, void* context);

// Parser debugging purpose(parsing time check, ...)
typedef void(*fl_msg_dbg_cb_on_parse_started_t)(const void* parser_handle);
typedef void(*fl_msg_dbg_cb_on_parse_ended_t)(const void* parser_handle);

FL_END_DECLS

#endif

