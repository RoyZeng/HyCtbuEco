using Aspose.BarCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class WxQRCode
{

    public static void CreatQR(string url , string path) {

        //二维码，微信可用
        BarCodeBuilder builder = new Aspose.BarCode.BarCodeBuilder(url, Symbology.QR);
        //hide code text
        builder.CodeLocation = CodeLocation.None;
        builder.RotationAngleF = 0;
        builder.Resolution = new Resolution(300, 300, ResolutionMode.Customized);
        builder.Save(path , BarCodeImageFormat.Png);
    }

}
