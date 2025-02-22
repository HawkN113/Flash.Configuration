﻿using Flash.Configuration.Core;

namespace Flash.Configuration.WinFormsApp.Configuration;

[FlashConfig("WinForms-CommonConfig")]
public class CommonConfig
{
    [FlashProperty("Property1")]
    [FlashValue("Property1_value")]
    public string Property1 { get; set; } = "Default value";
}