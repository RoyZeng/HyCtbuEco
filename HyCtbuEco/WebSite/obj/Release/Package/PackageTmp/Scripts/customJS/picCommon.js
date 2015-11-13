function PicGetMidPath(picOrgPath) {
    //根据orgPath，转为中图path
    if (picOrgPath == null) return;
    picOrgPath = picOrgPath.replace("~", "");
    picOrgPath = picOrgPath.replace("/Org/", "/Mid/");
    return picOrgPath;
}

function PicGetSmallPath(picOrgPath) {
    //根据orgPath，转为小图path
    if (picOrgPath == null) return;
    picOrgPath = picOrgPath.replace("~", "");
    picOrgPath = picOrgPath.replace("/Org/", "/Small/");
    return picOrgPath;
}
function PicGetBigPath(picOrgPath) {
    //根据orgPath，转为大图path
    if (picOrgPath == null) return;
    picOrgPath = picOrgPath.replace("~", "");
    picOrgPath = picOrgPath.replace("/Org/", "/Big/");
    return picOrgPath;
}

//去掉Webpath头部的~号
function CutWebPath(value) {
    if (value == null) return;
    return value.replace("~", "");
}
//取logo的小图
function getLogoSmall(picOrgPath) {

    if (picOrgPath == null) return;
    picOrgPath = picOrgPath.replace("~", "");
    var tmpA = picOrgPath.split(".");
    tmpA[tmpA.length - 2] = tmpA[tmpA.length - 2] + "_s";
    return tmpA.join(".");
}