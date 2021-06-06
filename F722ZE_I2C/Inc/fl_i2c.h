/*
 * fl_i2c.h
 *
 *  Created on: Jun 3, 2021
 *      Author: hcjung
 */

#ifndef FL_I2C_H
#define FL_I2C_H

#include "stm32f7xx_hal.h"
#include "fl_def.h"

#define FL_I2C_BUF_LEN    (8)

typedef struct _fl_i2c
{
  I2C_HandleTypeDef*  i2c;
  uint32_t            timeout;
  uint8_t             buf[FL_I2C_BUF_LEN];
  uint8_t             buf_len;
} fl_i2c_t;

FL_BEGIN_DECLS

FL_DECLARE(void) fl_i2c_init(fl_i2c_t *handle);
FL_DECLARE(fl_status_t) fl_i2c_read_byte(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint8_t *data);
FL_DECLARE(fl_status_t) fl_i2c_read_word(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint16_t *data);
FL_DECLARE(fl_status_t) fl_i2c_read_dword(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint32_t *data);
FL_DECLARE(fl_status_t) fl_i2c_write_byte(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint8_t data);
FL_DECLARE(fl_status_t) fl_i2c_write_word(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint16_t data);
FL_DECLARE(fl_status_t) fl_i2c_write_dword(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint32_t data);

FL_END_DECLS

#endif /* FL_I2C_H */
