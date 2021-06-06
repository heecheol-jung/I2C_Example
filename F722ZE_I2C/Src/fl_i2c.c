/*
 * fl_i2c.c
 *
 *  Created on: Jun 3, 2021
 *      Author: hcjung
 */
#include <string.h>
#include "fl_i2c.h"

FL_DECLARE(void) fl_i2c_init(fl_i2c_t *handle)
{
  memset(handle, 0, sizeof(fl_i2c_t));
}

FL_DECLARE(fl_status_t) fl_i2c_read_byte(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint8_t *data)
{
  fl_status_t ret = FL_OK;

  handle->buf[0] = (reg_addr >> 8);
  handle->buf[1] = (reg_addr & 0xFF);

  if (HAL_I2C_Master_Transmit(handle->i2c, i2c_addr, handle->buf, 2, handle->timeout) == HAL_OK)
  {
    if (HAL_I2C_Master_Receive(handle->i2c, i2c_addr, handle->buf, 1, handle->timeout) == HAL_OK)
    {
      *data = handle->buf[0];
    }
    else
    {
      ret = FL_ERROR; // Register read error.
    }
  }
  else
  {
    ret = FL_ERROR; // Address write error.
  }

  return ret;
}

FL_DECLARE(fl_status_t) fl_i2c_read_word(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint16_t *data)
{
  fl_status_t ret = FL_OK;

  handle->buf[0] = (reg_addr >> 8);
  handle->buf[1] = (reg_addr & 0xFF);

  if (HAL_I2C_Master_Transmit(handle->i2c, i2c_addr, handle->buf, 2, handle->timeout) == HAL_OK)
  {
    if (HAL_I2C_Master_Receive(handle->i2c, i2c_addr, handle->buf, 2, handle->timeout) == HAL_OK)
    {
      *data = ((uint16_t)handle->buf[0] << 8) | (uint16_t)handle->buf[1];
    }
    else
    {
      ret = FL_ERROR; // Register read error.
    }
  }
  else
  {
    ret = FL_ERROR; // Address write error.
  }

  return ret;
}

FL_DECLARE(fl_status_t) fl_i2c_read_dword(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint32_t *data)
{
  fl_status_t ret = FL_OK;

  handle->buf[0] = (reg_addr >> 8);
  handle->buf[1] = (reg_addr & 0xFF);

  if (HAL_I2C_Master_Transmit(handle->i2c, i2c_addr, handle->buf, 2, handle->timeout) == HAL_OK)
  {
    if (HAL_I2C_Master_Receive(handle->i2c, i2c_addr, handle->buf, 4, handle->timeout) == HAL_OK)
    {
      *data = ((uint32_t)handle->buf[0] << 24) |
              ((uint32_t)handle->buf[1] << 16) |
              ((uint32_t)handle->buf[2] << 8)  |
              (uint32_t)handle->buf[3];
    }
    else
    {
      ret = FL_ERROR; // Register read error.
    }
  }
  else
  {
    ret = FL_ERROR; // Address write error.
  }

  return ret;
}

FL_DECLARE(fl_status_t) fl_i2c_write_byte(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint8_t data)
{
  fl_status_t ret = FL_OK;

  handle->buf[0] = (reg_addr >> 8);
  handle->buf[1] = (reg_addr & 0xFF);
  handle->buf[2] = data;

  if (HAL_I2C_Master_Transmit(handle->i2c, i2c_addr, handle->buf, 3, handle->timeout) != HAL_OK)
  {
    ret = FL_ERROR; // Write error.
  }

  return ret;
}

FL_DECLARE(fl_status_t) fl_i2c_write_word(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint16_t data)
{
  fl_status_t ret = FL_OK;

  handle->buf[0] = (reg_addr >> 8);
  handle->buf[1] = (reg_addr & 0xFF);
  handle->buf[2] = (data >> 8);
  handle->buf[3] = data & 0xFF;

  if (HAL_I2C_Master_Transmit(handle->i2c, i2c_addr, handle->buf, 4, handle->timeout) != HAL_OK)
  {
    ret = FL_ERROR; // Write error.
  }

  return ret;
}

FL_DECLARE(fl_status_t) fl_i2c_write_dword(fl_i2c_t *handle, uint8_t i2c_addr, uint16_t reg_addr, uint32_t data)
{
  fl_status_t ret = FL_OK;

  handle->buf[0] = (reg_addr >> 8);
  handle->buf[1] = (reg_addr & 0xFF);
  handle->buf[2] = (data >> 24);
  handle->buf[3] = ((data >> 16) & 0xFF);
  handle->buf[4] = ((data >> 8) & 0xFF);
  handle->buf[5] = (data & 0xFF);

  if (HAL_I2C_Master_Transmit(handle->i2c, i2c_addr, handle->buf, 6, handle->timeout) != HAL_OK)
  {
    ret = FL_ERROR; // Write error.
  }

  return ret;
}
