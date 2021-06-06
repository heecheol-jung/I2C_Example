using RegisterCore.Net.Models;
using RegisterSqlite.Net;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace I2CWpfApp
{
    public static class AppUtil
    {
        public static void CreateDefaultDb()
        {
            if (!File.Exists(RegisterContext.VL6180X_DB_NAME))
            {
                Log.Warning("No database file");

                using (var ctx = new RegisterContext())
                {
                    ctx.Database.EnsureCreated();

                    var manufacturer = new Manufacturer()
                    {
                        Name = "STMicroelectronics",
                        HomePage = "https://www.st.com"
                    };

                    var vl6180x = new Chip()
                    {
                        ChipType = "Sensor",
                        Name = "VL6180x",
                        Description = "Distance sensor"
                    };
                    AddVl6180xRegisters(vl6180x);

                    manufacturer.Chips.Add(vl6180x);
                    ctx.Add(manufacturer);
                    ctx.SaveChanges();
                }

                Log.Debug("Database file created");
            }
        }

        public static List<(ulong address, uint value)> ReadRegisterValuesFromFile(string fileName)
        {
            string line;
            char[] seperator = new char[] { ',' };
            List<(ulong address, uint value)> regValues = new List<(ulong address, uint value)>();

            try
            {
                StreamReader sr = new StreamReader(fileName);
                while ((line = sr.ReadLine()) != null)
                {
                    string[] args = line.Split(seperator);
                    if (args?.Length == 2)
                    {
                        if (ulong.TryParse(args[0], NumberStyles.HexNumber, null, out ulong addr) == true)
                        {
                            if (uint.TryParse(args[1], NumberStyles.HexNumber, null, out uint val) == true)
                            {
                                regValues.Add((addr, val));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

            return regValues;
        }

        public static void SaveRegisterValues(string fileName, ObservableCollection<Register> registers)
        {
            StreamWriter sw = new StreamWriter(fileName);

            foreach (var reg in registers)
            {
                if (reg.Bits <= 8)
                {
                    sw.WriteLine($"{reg.Address:X8},{reg.Value:X2}");
                }
                else if (reg.Bits <= 16)
                {
                    sw.WriteLine($"{reg.Address:X8},{reg.Value:X4}");
                }
                else if (reg.Bits <= 32)
                {
                    sw.WriteLine($"{reg.Address:X8},{reg.Value:X8}");
                }
                else
                {
                    sw.WriteLine($"{reg.Address:X8},{reg.Value:X}");
                }
            }

            sw.Close();
        }

        #region VL6180x register add
        private static void AddVl6180xRegisters(Chip chip)
        {
            Add_000_Register(chip);
            Add_001_Register(chip);
            Add_002_Register(chip);
            Add_003_Register(chip);
            Add_004_Register(chip);
            Add_006_Register(chip);
            Add_007_Register(chip);
            Add_008_Register(chip);
            Add_010_Register(chip);
            Add_011_Register(chip);
            Add_012_Register(chip);
            Add_014_Register(chip);
            Add_015_Register(chip);
            Add_016_Register(chip);
            Add_017_Register(chip);
            Add_018_Register(chip);
            Add_019_Register(chip);
            Add_01A_Register(chip);
            Add_01B_Register(chip);
            Add_01C_Register(chip);
            Add_01E_Register(chip);
            Add_021_Register(chip);
            Add_022_Register(chip);
            Add_024_Register(chip);
            Add_025_Register(chip);
            Add_026_Register(chip);
            Add_02C_Register(chip);
            Add_02D_Register(chip);
            Add_02E_Register(chip);
            Add_031_Register(chip);
            Add_038_Register(chip);
            Add_03A_Register(chip);
            Add_03C_Register(chip);
            Add_03E_Register(chip);
            Add_03F_Register(chip);
            Add_040_Register(chip);
            Add_04D_Register(chip);
            Add_04E_Register(chip);
            Add_04F_Register(chip);
            Add_050_Register(chip);
            Add_052_05E_Registers(chip);
            Add_062_Register(chip);
            Add_064_Register(chip);
            Add_066_Register(chip);
            Add_068_Register(chip);
            Add_06C_Register(chip);
            Add_070_Register(chip);
            Add_074_Register(chip);
            Add_078_Register(chip);
            Add_07C_Register(chip);
            Add_080_Register(chip);
            Add_10A_Register(chip);
            Add_119_Register(chip);
            Add_120_Register(chip);
            Add_212_Register(chip);
            Add_2A3_Register(chip);
        }

        private static void Add_2A3_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "INTERLEAVED_MODE__ENABLE",
                Bits = 8,
                Address = 0x2A3,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "interleaved_mode__enable",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Write 0x1 to this register to select ALS+Range interleaved mode.
Use SYSALS__START and SYSALS__INTERMEASUREMENT_PERIOD to control this mode.
A range measurement is automatically performed immediately after each ALS measurement."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_212_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "I2C_SLAVE__DEVICE_ADDRESS",
                Bits = 8,
                Address = 0x212,
                ResetValue = "0x29"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "super_i2c_slave__device_address",
                Offset = 0,
                Bits = 7,
                AccessType = BitAccessType.ReadWrite,
                Description = @"User programmable I2C address (7-bit). Device address can be re-designated after power-up."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 7,
                Bits = 1,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_120_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "FIRMWARE__RESULT_SCALER",
                Bits = 8,
                Address = 0x120,
                ResetValue = "0x1"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "firmware__als_result_scaler",
                Offset = 0,
                Bits = 4,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Bits [3:0] analogue gain 1 to 16x"
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 4,
                Bits = 4,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_119_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "FIRMWARE__BOOTUP",
                Bits = 8,
                Address = 0x119,
                ResetValue = "0x1"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "firmware__bootup",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"FW must set bit once initial boot has been completed."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 1,
                Bits = 7,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_10A_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "READOUT__AVERAGING_SAMPLE_PERIOD",
                Bits = 8,
                Address = 0x10A,
                ResetValue = "0x30"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "readout__averaging_sample_period",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"The internal readout averaging sample period can be
adjusted from 0 to 255. Increasing the sampling period decreases noise but also reduces the
effective max convergence time and increases power consumption:
Effective max convergence time = max convergence time - readout averaging period (see
Section 2.7.1: Range timing). Each unit sample period corresponds to around 64.5 μs
additional processing time. The recommended setting is 48 which equates to around 4.3 ms."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_080_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__RANGE_REFERENCE_CONV_TIME",
                Bits = 32,
                Address = 0x080,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__range_reference_conv_time",
                Offset = 0,
                Bits = 32,
                AccessType = BitAccessType.ReadOnly,
                Description = @"sensor count output value attributed to signal on the Reference array."
            };
            r.BitFields.Add(bf1);
        }

        private static void Add_07C_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__RANGE_RETURN_CONV_TIME",
                Bits = 32,
                Address = 0x07C,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__range_return_conv_time",
                Offset = 0,
                Bits = 32,
                AccessType = BitAccessType.ReadOnly,
                Description = @"sensor count output value attributed to signal on the Return array."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_078_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__RANGE_REFERENCE_AMB_COUNT",
                Bits = 32,
                Address = 0x078,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__range_reference_amb_coun",
                Offset = 0,
                Bits = 32,
                AccessType = BitAccessType.ReadOnly,
                Description = @"sensor count output value attributed to uncorrelated
ambient signal on the Reference array."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_074_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__RANGE_RETURN_AMB_COUNT",
                Bits = 32,
                Address = 0x074,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__range_return_amb_count",
                Offset = 0,
                Bits = 32,
                AccessType = BitAccessType.ReadOnly,
                Description = @"sensor count output value attributed to uncorrelated ambient
signal on the Return array. Must be multiplied by 6 if used to calculate the ambient to signal
threshold."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_070_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__RANGE_REFERENCE_SIGNAL_COUNT",
                Bits = 32,
                Address = 0x070,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__range_reference_signal_count",
                Offset = 0,
                Bits = 32,
                AccessType = BitAccessType.ReadOnly,
                Description = @"sensor count output value attributed to signal correlated to IR emitter on the Reference array."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_06C_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__RANGE_RETURN_SIGNAL_COUNT",
                Bits = 32,
                Address = 0x06C,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__range_return_signal_count",
                Offset = 0,
                Bits = 32,
                AccessType = BitAccessType.ReadOnly,
                Description = @"sensor count output value attributed to signal correlated to IR emitter on the Return array."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_068_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__RANGE_REFERENCE_RATE",
                Bits = 16,
                Address = 0x068,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__range_reference_rate",
                Offset = 0,
                Bits = 16,
                AccessType = BitAccessType.ReadOnly,
                Description = @"sensor count rate of reference signal returns. Computed from
REFERENCE_SIGNAL_COUNT / RETURN_CONV_TIME. Mcps 9.7 format
Note: Both arrays converge at the same time, so using the return array convergence time is
correct."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_066_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__RANGE_RETURN_RATE",
                Bits = 16,
                Address = 0x066,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__range_return_rate",
                Offset = 0,
                Bits = 16,
                AccessType = BitAccessType.ReadOnly,
                Description = @"sensor count rate of signal returns correlated to IR emitter.
Computed from RETURN_SIGNAL_COUNT / RETURN_CONV_TIME. Mcps 9.7 format"
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_064_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__RANGE_RAW",
                Bits = 8,
                Address = 0x064,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__range_val",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Raw Range result value with offset applied (no cross-talk compensation applied). Unit is in mm."
            };
            r.BitFields.Add(bf1);
        }

        private static void Add_062_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__RANGE_VAL",
                Bits = 8,
                Address = 0x062,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__range_val",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Final range result value presented to the user for use. Unit is in mm."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_050_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__ALS_VAL",
                Bits = 16,
                Address = 0x050,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__als_ambient_light",
                Offset = 0,
                Bits = 16,
                AccessType = BitAccessType.ReadOnly,
                Description = @"16 Bit ALS count output value. Lux value depends on Gain and
integration settings and calibrated lux/count setting."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_052_05E_Registers(Chip chip)
        {
            ulong startAddress = 0x052;

            for (int i = 0; i < 8; i++)
            {
                RegisterTemplate r = new RegisterTemplate()
                {
                    Name = $"RESULT__HISTORY_BUFFER_{i}",
                    Bits = 16,
                    Address = (startAddress + (ulong)i * 2),
                    ResetValue = "0x0"
                };

                // google search : c# string new line carriage return
                // https://stackoverflow.com/questions/6806841/how-can-i-create-a-carriage-return-in-my-c-sharp-string/6806871
                BitFieldTemplate bf1 = new BitFieldTemplate()
                {
                    Name = $"result__history_buffer_{i}",
                    Offset = 0,
                    Bits = 16,
                    AccessType = BitAccessType.ReadOnly,
                    Description = "Range/ALS result value." + Environment.NewLine + $"Range mode; Bits[15:8] range_val_latest; Bits[7:0] range_val_d{i * 2 + 1};" + Environment.NewLine
                };

                if (i == 0)
                {
                    bf1.Description += "ALS mode; Bits[15:0] als_val_latest";
                }
                else
                {
                    bf1.Description += "ALS mode; Bits[15:0] als_val_d" + $"{i}";
                }

                r.BitFields.Add(bf1);

                chip.Registers.Add(r);
            }
        }

        private static void Add_04F_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__INTERRUPT_STATUS_GPIO",
                Bits = 8,
                Address = 0x04F,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result_int_range_gpio",
                Offset = 0,
                Bits = 3,
                AccessType = BitAccessType.ReadOnly,
                Description = @"result_int_range_gpio: Interrupt bits for Range:
0: No threshold events reported
1: Level Low threshold event
2: Level High threshold event
3: Out Of Window threshold event
4: New Sample Ready threshold event"
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "result_int_als_gpio",
                Offset = 3,
                Bits = 3,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Interrupt bits for ALS:
0: No threshold events reported
1: Level Low threshold event
2: Level High threshold event
3: Out Of Window threshold event
4: New Sample Ready threshold event"
            };
            r.BitFields.Add(bf2);

            BitFieldTemplate bf3 = new BitFieldTemplate()
            {
                Name = "result_int_error_gpio",
                Offset = 6,
                Bits = 2,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Interrupt bits for Error:
0: No error reported
1: Laser Safety Error
2: PLL error (either PLL1 or PLL2)"
            };
            r.BitFields.Add(bf3);

            chip.Registers.Add(r);
        }

        private static void Add_04E_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__ALS_STATUS",
                Bits = 8,
                Address = 0x04E,
                ResetValue = "0x1"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__als_device_ready",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Device Ready. When set to 1, indicates the device mode and
configuration can be changed and a new start command will be accepted. When 0, indicates
the device is busy."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "result__als_measurement_ready",
                Offset = 1,
                Bits = 1,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Legacy register - DO NOT USE
Use instead 6.2.39: RESULT__INTERRUPT_STATUS_GPIO"
            };
            r.BitFields.Add(bf2);

            BitFieldTemplate bf3 = new BitFieldTemplate()
            {
                Name = "result__als_max_threshold_hit",
                Offset = 2,
                Bits = 1,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Legacy register - DO NOT USE
Use instead 6.2.39: RESULT__INTERRUPT_STATUS_GPIO"
            };
            r.BitFields.Add(bf3);

            BitFieldTemplate bf4 = new BitFieldTemplate()
            {
                Name = "result__als_min_threshold_hit",
                Offset = 3,
                Bits = 1,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Legacy register - DO NOT USE
Use instead 6.2.39: RESULT__INTERRUPT_STATUS_GPIO"
            };
            r.BitFields.Add(bf4);

            BitFieldTemplate bf5 = new BitFieldTemplate()
            {
                Name = "result__als_error_code",
                Offset = 4,
                Bits = 4,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Specific error and debug codes
0000: No error
0001: Overflow error
0002: Underflow error"
            };
            r.BitFields.Add(bf5);

            chip.Registers.Add(r);
        }

        private static void Add_04D_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "RESULT__RANGE_STATUS",
                Bits = 8,
                Address = 0x04D,
                ResetValue = "0x1"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "result__range_device_ready",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Device Ready. When set to 1, indicates the device mode and
configuration can be changed and a new start command will be accepted. When 0, indicates
the device is busy"
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "result__range_measurement_ready",
                Offset = 1,
                Bits = 1,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Legacy register - DO NOT USE
Use instead 6.2.39: RESULT__INTERRUPT_STATUS_GPIO"
            };
            r.BitFields.Add(bf2);

            BitFieldTemplate bf3 = new BitFieldTemplate()
            {
                Name = "result__range_max_threshold_hit",
                Offset = 2,
                Bits = 1,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Legacy register - DO NOT USE
Use instead 6.2.39: RESULT__INTERRUPT_STATUS_GPIO"
            };
            r.BitFields.Add(bf3);

            BitFieldTemplate bf4 = new BitFieldTemplate()
            {
                Name = "result__range_min_threshold_hit",
                Offset = 3,
                Bits = 1,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Legacy register - DO NOT USE
Use instead 6.2.39: RESULT__INTERRUPT_STATUS_GPIO"
            };
            r.BitFields.Add(bf4);

            BitFieldTemplate bf5 = new BitFieldTemplate()
            {
                Name = "result__range_error_code",
                Offset = 4,
                Bits = 4,
                AccessType = BitAccessType.ReadOnly,
                Description = @"Specific error codes
0000: No error
0001: VCSEL Continuity Test
0010: VCSEL Watchdog Test
0011: VCSEL Watchdog
0100: PLL1 Lock
0101: PLL2 Lock
0110: Early Convergence Estimate
0111: Max Convergence
1000: No Target Ignore
1001: Not used
1010: Not used
1011: Max Signal To Noise Ratio
1100: Raw Ranging Algo Underflow
1101: Raw Ranging Algo Overflow
1110: Ranging Algo Underflow
1111: Ranging Algo Overflow"
            };
            r.BitFields.Add(bf5);

            chip.Registers.Add(r);
        }

        private static void Add_040_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSALS__INTEGRATION_PERIOD",
                Bits = 16,
                Address = 0x040,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysals__integration_period",
                Offset = 0,
                Bits = 9,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Integration period for ALS mode. 1 code = 1 ms (0 = 1 ms).
Recommended setting is 100 ms (0x63)."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 9,
                Bits = 7,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_03F_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSALS__ANALOGUE_GAIN",
                Bits = 8,
                Address = 0x03F,
                ResetValue = "0x06"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysals__analogue_gain_light",
                Offset = 0,
                Bits = 3,
                AccessType = BitAccessType.ReadWrite,
                Description = @"ALS analogue gain (light channel)
0: ALS Gain = 20
1: ALS Gain = 10
2: ALS Gain = 5.0
3: ALS Gain = 2.5
4: ALS Gain = 1.67
5: ALS Gain = 1.25
6: ALS Gain = 1.0
7: ALS Gain = 40
Controls the “light” channel gain.
Note: Upper nibble should be set to 0x4 i.e. For ALS gain of 1.0 write 0x46."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 3,
                Bits = 5,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_03E_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSALS__INTERMEASUREMENT_PERIOD",
                Bits = 8,
                Address = 0x03E,
                ResetValue = "0xFF"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysals__intermeasurement_period",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Time delay between measurements in ALS continuous mode. Range 0-254 (0 = 10ms). Step size = 10ms."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_03C_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSALS__THRESH_LOW",
                Bits = 16,
                Address = 0x03C,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysals__thresh_low",
                Offset = 0,
                Bits = 16,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Low Threshold value for ALS comparison. Range 0-65535 codes."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_03A_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSALS__THRESH_HIGH",
                Bits = 16,
                Address = 0x03A,
                ResetValue = "0xFFFF"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysals__thresh_high",
                Offset = 0,
                Bits = 16,
                AccessType = BitAccessType.ReadWrite,
                Description = @"High Threshold value for ALS comparison. Range 0-65535 codes."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_038_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSALS__START",
                Bits = 8,
                Address = 0x038,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysals__startstop",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Start/Stop trigger based on current mode and system configuration of
device_ready. FW clears register automatically.
Setting this bit to 1 in single-shot mode starts a single measurement.
Setting this bit to 1 in continuous mode will either start continuous operation (if stopped) or halt
continuous operation (if started).
This bit is auto-cleared in both modes of operation.
See 6.2.56: INTERLEAVED_MODE__ENABLE for combined ALS and Range operation."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "sysals__mode_select",
                Offset = 1,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Device Mode select
0: ALS Mode Single-Shot
1: ALS Mode Continuous"
            };
            r.BitFields.Add(bf2);

            BitFieldTemplate bf3 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 2,
                Bits = 6,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf3);

            chip.Registers.Add(r);
        }

        private static void Add_031_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__VHV_REPEAT_RATE",
                Bits = 8,
                Address = 0x031,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__vhv_repeat_rate",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"User entered repeat rate of auto VHV task (0 = off, 255 = after every 255 measurements)"
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_02E_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__VHV_RECALIBRATE",
                Bits = 8,
                Address = 0x02E,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__vhv_recalibrate",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"User-Controlled enable bit to force FW to carry out recalibration of
the VHV setting for sensor array. FW clears bit after operation carried out.
0: Disabled
1: Manual trigger for VHV recalibration. Can only be called when ALS and ranging are in STOP
mode"
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "sysrange__vhv_status",
                Offset = 1,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"FW controlled status bit showing when FW has completed auto-vhv process.
0: FW has finished autoVHV operation
1: During autoVHV operation"
            };
            r.BitFields.Add(bf2);

            BitFieldTemplate bf3 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 2,
                Bits = 6,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf3);

            chip.Registers.Add(r);
        }

        private static void Add_02D_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__RANGE_CHECK_ENABLES",
                Bits = 8,
                Address = 0x02D,
                ResetValue = "0x11",
                Description = "register default overwritten at boot-up by NVM contents."
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__early_convergence_enable",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Measurement enable/disable"
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "sysrange__range_ignore_enable",
                Offset = 1,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Measurement enable/disable"
            };
            r.BitFields.Add(bf2);

            BitFieldTemplate bf3 = new BitFieldTemplate()
            {
                Name = "0",
                Offset = 2,
                Bits = 1,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf3);

            BitFieldTemplate bf4 = new BitFieldTemplate()
            {
                Name = "0",
                Offset = 3,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite
            };
            r.BitFields.Add(bf4);

            BitFieldTemplate bf5 = new BitFieldTemplate()
            {
                Name = "sysrange__signal_to_noise_enab",
                Offset = 4,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Measurement enable/disable"
            };
            r.BitFields.Add(bf5);

            BitFieldTemplate bf6 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 5,
                Bits = 3,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf6);

            chip.Registers.Add(r);
        }

        private static void Add_02C_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__MAX_AMBIENT_LEVEL_MULT",
                Bits = 8,
                Address = 0x02C,
                ResetValue = "0xA0",
                Description = "register default overwritten at boot-up by NVM contents."
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__max_ambient_level_mult",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"User input value to multiply return_signal_count for AMB:signal ratio check. 
If (amb counts * 6) > return_signal_count * mult then abandon measurement due to high ambient (4.4 format)."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_026_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__RANGE_IGNORE_THRESHOLD",
                Bits = 16,
                Address = 0x026,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__range_ignore_threshold",
                Offset = 0,
                Bits = 16,
                AccessType = BitAccessType.ReadWrite,
                Description = @"User configurable min threshold signal return rate. 
Used to filter out ranging due to cover glass when there is no target above the device. Mcps 9.7 format.
Note: Register must be initialized if this feature is used."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_025_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__RANGE_IGNORE_VALID_HEIGHT",
                Bits = 8,
                Address = 0x025,
                ResetValue = "0x0",
                Description = "register default overwritten at boot-up by NVM contents."
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__range_ignore_valid_height",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Range below which ignore threshold is applied. Aim is
to ignore the cover glass i.e. low signal rate at near distance. Should not be applied to low
reflectance target at far distance. Range in mm.
Note: It is recommended to set this register to 255 if the range ignore feature is used."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_024_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__PART_TO_PART_RANGE_OFFSET",
                Bits = 8,
                Address = 0x024,
                ResetValue = "0xYY",
                Description = "register default overwritten at boot-up by NVM contents."
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__part_to_part_range_offset",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"2s complement format."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_022_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__EARLY_CONVERGENCE_ESTIMATE",
                Bits = 16,
                Address = 0x022,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__early_convergence_estimate",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"FW carries out an estimate of convergence rate 0.5ms into each new range measurement. If
convergence rate is below user input value, the operation aborts to save power.
Note: This register must be configured otherwise ECE should be disabled via
SYSRANGE__RANGE_CHECK_ENABLES."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_021_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__CROSSTALK_VALID_HEIGHT",
                Bits = 8,
                Address = 0x021,
                ResetValue = "0x14"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__crosstalk_valid_height",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Minimum range value in mm to qualify for cross-talk compensation."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_01E_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__CROSSTALK_COMPENSATION_RATE",
                Bits = 16,
                Address = 0x01E,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__crosstalk_compensation_rate",
                Offset = 0,
                Bits = 16,
                AccessType = BitAccessType.ReadWrite,
                Description = @"User-controlled crosstalk compensation in Mcps (9.7 format)."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_01C_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__MAX_CONVERGENCE_TIME",
                Bits = 8,
                Address = 0x01C,
                ResetValue = "0x31"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__max_convergence_time",
                Offset = 0,
                Bits = 6,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Maximum time to run measurement in Ranging modes.
Range 1 - 63 ms (1 code = 1 ms); Measurement aborted when limit reached to aid power reduction. 
For example, 0x01 = 1ms, 0x0a = 10ms.
Note: Effective max_convergence_time depends on readout_averaging_sample_period
setting."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 6,
                Bits = 2,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_01B_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__INTERMEASUREMENT_PERIOD",
                Bits = 8,
                Address = 0x01B,
                ResetValue = "0xFF"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__intermeasurement_period",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Time delay between measurements in Ranging continuous mode. Range 0-254 (0 = 10ms). Step size = 10ms."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_01A_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__THRESH_LOW",
                Bits = 8,
                Address = 0x01A,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__thresh_low",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Low Threshold value for ranging comparison. Range 0-255mm."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_019_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__THRESH_HIGH",
                Bits = 8,
                Address = 0x019,
                ResetValue = "0xFF"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__thresh_high",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                Description = @"High Threshold value for ranging comparison. Range 0-255mm."
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_018_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSRANGE__START",
                Bits = 8,
                Address = 0x018,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "sysrange__startstop",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"StartStop trigger based on current mode and system configuration of
device_ready. FW clears register automatically.
Setting this bit to 1 in single-shot mode starts a single measurement.
Setting this bit to 1 in continuous mode will either start continuous operation (if stopped) or halt
continuous operation (if started).
This bit is auto-cleared in both modes of operation."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "sysrange__mode_select",
                Offset = 1,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Device Mode select
0: Ranging Mode Single-Shot
1: Ranging Mode Continuous"
            };
            r.BitFields.Add(bf2);

            BitFieldTemplate bf3 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 2,
                Bits = 6,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf3);

            chip.Registers.Add(r);
        }

        private static void Add_017_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSTEM__GROUPED_PARAMETER_HOLD",
                Bits = 8,
                Address = 0x017,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "grouped_parameter_hold",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Flag set over I2C to indicate that data is being updated
0: Data is stable - FW is safe to copy
1: Data being updated - FW not safe to copy
Usage: set to 0x01 first, write any of the registers listed below, then set to 0x00 so that the
settings are used by the firmware at the start of the next measurement.
SYSTEM__INTERRUPT_CONFIG_GPIO
SYSRANGE__THRESH_HIGH
SYSRANGE__THRESH_LOW
SYSALS__INTEGRATION_PERIOD
SYSALS__ANALOGUE_GAIN
SYSALS__THRESH_HIGH
SYSALS__THRESH_LOW"
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 1,
                Bits = 7,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_016_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSTEM__FRESH_OUT_OF_RESET",
                Bits = 8,
                Address = 0x016,
                ResetValue = "0x1"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "fresh_out_of_reset",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Fresh out of reset bit, default of 1, user can set this to 0 after initial boot and can therefore use this to check for a reset condition"
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 1,
                Bits = 7,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_015_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSTEM__INTERRUPT_CLEAR",
                Bits = 8,
                Address = 0x015,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "int_clear_sig",
                Offset = 0,
                Bits = 3,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Interrupt clear bits. Writing a 1 to each bit will clear the intended interrupt.
Bit [0] - Clear Range Int
Bit [1] - Clear ALS Int
Bit [2] - Clear Error Int."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 3,
                Bits = 5,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_014_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSTEM__INTERRUPT_CONFIG_GPIO",
                Bits = 8,
                Address = 0x014,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "range_int_mode",
                Offset = 0,
                Bits = 3,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Interrupt mode source for Range readings:
0: Disabled
1: Level Low (value < thresh_low)
2: Level High (value > thresh_high)
3: Out Of Window (value < thresh_low OR value > thresh_high)
4: New sample ready"
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "als_int_mode",
                Offset = 3,
                Bits = 3,
                AccessType = BitAccessType.ReadWrite,
                Description = @"Interrupt mode source for ALS readings:
0: Disabled
1: Level Low (value < thresh_low)
2: Level High (value > thresh_high)
3: Out Of Window (value < thresh_low OR value > thresh_high)
4: New sample ready"
            };
            r.BitFields.Add(bf2);

            BitFieldTemplate bf3 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 6,
                Bits = 2,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf3);

            chip.Registers.Add(r);
        }

        private static void Add_012_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSTEM__HISTORY_CTRL",
                Bits = 8,
                Address = 0x012,
                ResetValue = "0x0"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "system__history_buffer_enable",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = "Enable History buffering.\r\n0: Disabled\r\n1: Enabled"
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "system__history_buffer_mode",
                Offset = 1,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = "Select mode buffer results for:\r\n0: Ranging (stores the last 8 ranging values (8-bit)\r\n1: ALS (stores the last 8 ALS values (16-bit)"
            };
            r.BitFields.Add(bf2);

            BitFieldTemplate bf3 = new BitFieldTemplate()
            {
                Name = "system__history_buffer_clear",
                Offset = 2,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = "User-command to clear history (FW will auto-clear this bit when clear has completed).\r\n0: Disabled\r\n1: Clear all history buffers"
            };
            r.BitFields.Add(bf3);

            BitFieldTemplate bf4 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 3,
                Bits = 5,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf4);

            chip.Registers.Add(r);
        }

        private static void Add_011_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSTEM__MODE_GPIO1",
                Bits = 8,
                Address = 0x011,
                ResetValue = "0x20"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = "Reserved. Write 0."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "system__gpio1_select",
                Offset = 1,
                Bits = 4,
                AccessType = BitAccessType.ReadWrite,
                Description = "Functional configuration options.\r\n0000: OFF (Hi-Z)\r\n1000: GPIO Interrupt output"
            };
            r.BitFields.Add(bf2);

            BitFieldTemplate bf3 = new BitFieldTemplate()
            {
                Name = "system__gpio1_polarity",
                Offset = 5,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = "Signal Polarity Selection.\r\n0: Active-low\r\n1: Active-high"
            };
            r.BitFields.Add(bf3);

            BitFieldTemplate bf4 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 6,
                Bits = 2,
                AccessType = BitAccessType.ReadOnly
            };
            r.BitFields.Add(bf4);

            chip.Registers.Add(r);
        }

        private static void Add_010_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "SYSTEM__MODE_GPIO0",
                Bits = 8,
                Address = 0x010,
                ResetValue = "0x60"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 0,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = "Reserved. Write 0."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "system__gpio0_select",
                Offset = 1,
                Bits = 4,
                AccessType = BitAccessType.ReadWrite,
                Description = "Functional configuration options.\r\n0000: OFF (Hi-Z)\r\n1000: GPIO Interrupt output"
            };
            r.BitFields.Add(bf2);

            BitFieldTemplate bf3 = new BitFieldTemplate()
            {
                Name = "system__gpio0_polarity",
                Offset = 5,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = "Signal Polarity Selection.\r\n0: Active-low\r\n1: Active-high"
            };
            r.BitFields.Add(bf3);

            BitFieldTemplate bf4 = new BitFieldTemplate()
            {
                Name = "system__gpio0_is_xshutdown",
                Offset = 6,
                Bits = 1,
                AccessType = BitAccessType.ReadWrite,
                Description = "Priority mode - when enabled, other bits of the register are ignored.GPIO0 is main XSHUTDOWN input.\r\n0: Disabled\r\n1: Enabled - GPIO0 is main XSHUTDOWN input."
            };
            r.BitFields.Add(bf4);

            BitFieldTemplate bf5 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 7,
                Bits = 1,
                AccessType = BitAccessType.ReadOnly,
            };
            r.BitFields.Add(bf5);

            chip.Registers.Add(r);
        }

        private static void Add_008_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "IDENTIFICATION__TIME",
                Bits = 16,
                Address = 0x008,
                ResetValue = "0xYYYY",
                Description = "register default overwritten at boot-up by NVM contents.\r\nPart of the register set that can be used to uniquely identify a module."
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "identification__time",
                Offset = 0,
                Bits = 16,
                AccessType = BitAccessType.ReadWrite,
                Description = "Time since midnight (in seconds) = register_value * 2"
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }

        private static void Add_007_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "IDENTIFICATION__DATE_LO",
                Bits = 8,
                Address = 0x007,
                ResetValue = "0xYY",
                Description = "register default overwritten at boot-up by NVM contents.\r\nPart of the register set that can be used to uniquely identify a module."
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "identification__phase",
                Offset = 0,
                Bits = 3,
                AccessType = BitAccessType.ReadWrite,
                Description = "Manufacturing phase identification (bits[2:0])."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "identification__day",
                Offset = 3,
                Bits = 5,
                AccessType = BitAccessType.ReadWrite,
                Description = "Manufacturing day (bits[4:0])."
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_006_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "IDENTIFICATION__DATE_HI",
                Bits = 8,
                Address = 0x006,
                ResetValue = "0xYY",
                Description = "register default overwritten at boot-up by NVM contents.\r\nPart of the register set that can be used to uniquely identify a module."
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "identification__month",
                Offset = 0,
                Bits = 4,
                AccessType = BitAccessType.ReadWrite,
                Description = "Manufacturing month (bits[3:0])."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "identification__year",
                Offset = 4,
                Bits = 4,
                AccessType = BitAccessType.ReadWrite,
                Description = "Last digit of manufacturing year (bits[3:0])."
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_004_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "IDENTIFICATION__MODULE_REV_MINOR",
                Bits = 8,
                Address = 0x004,
                ResetValue = "0x2",
                Description = "register default overwritten at boot-up by NVM contents."
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "identification__module_rev_minor",
                Offset = 0,
                Bits = 3,
                AccessType = BitAccessType.ReadWrite,
                ResetValue = "0x2",
                Description = "Revision identifier of the Module Package for minor change.\r\nUsed to store NVM content version. Contact ST for current information."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 3,
                Bits = 5,
                AccessType = BitAccessType.ReadOnly,
                ResetValue = "0x0"
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_003_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "IDENTIFICATION__MODULE_REV_MAJOR",
                Bits = 8,
                Address = 0x003,
                ResetValue = "0x1",
                Description = "register default overwritten at boot-up by NVM contents."
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "identification__module_rev_major",
                Offset = 0,
                Bits = 3,
                AccessType = BitAccessType.ReadWrite,
                ResetValue = "0x1",
                Description = "Revision identifier of the Module Package for major change.\r\nUsed to store NVM content version. Contact ST for current information."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 3,
                Bits = 5,
                AccessType = BitAccessType.ReadOnly,
                ResetValue = "0x0"
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_002_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "IDENTIFICATION__MODEL_REV_MINOR",
                Bits = 8,
                Address = 0x002,
                ResetValue = "0x3",
                Description = "register default overwritten at boot-up by NVM contents."
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "identification__model_rev_minor",
                Offset = 0,
                Bits = 3,
                AccessType = BitAccessType.ReadWrite,
                ResetValue = "0x1",
                Description = "Revision identifier of the Device for minor change.\r\nIDENTIFICATION__MODEL_REV_MINOR = 3 for latest ROM revision"
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 3,
                Bits = 5,
                AccessType = BitAccessType.ReadOnly,
                ResetValue = "0x0"
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_001_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "IDENTIFICATION__MODEL_REV_MAJOR",
                Bits = 8,
                Address = 0x001,
                ResetValue = "0x1",
                Description = "register default overwritten at boot-up by NVM contents"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "identification__model_rev_major",
                Offset = 0,
                Bits = 3,
                AccessType = BitAccessType.ReadWrite,
                ResetValue = "0x1",
                Description = "Revision identifier of the Device for major change."
            };
            r.BitFields.Add(bf1);

            BitFieldTemplate bf2 = new BitFieldTemplate()
            {
                Name = "RESERVED",
                Offset = 3,
                Bits = 5,
                AccessType = BitAccessType.ReadOnly,
                ResetValue = "0x0"
            };
            r.BitFields.Add(bf2);

            chip.Registers.Add(r);
        }

        private static void Add_000_Register(Chip chip)
        {
            RegisterTemplate r = new RegisterTemplate()
            {
                Name = "IDENTIFICATION__MODEL_ID",
                Bits = 8,
                Address = 0x000,
                ResetValue = "0xB4"
            };

            BitFieldTemplate bf1 = new BitFieldTemplate()
            {
                Name = "identification__model_id",
                Offset = 0,
                Bits = 8,
                AccessType = BitAccessType.ReadWrite,
                ResetValue = "0xB4"
            };
            r.BitFields.Add(bf1);

            chip.Registers.Add(r);
        }
        #endregion
    }
}
