function SysChangePassWordRunPage() {
    //当前密码验证
    $("#CurrentPassword").blur(function () {
        var CurrentPassword = $(this).val();
        $.ajax({
            type: "post",
            url: "/WebApi/SysManager/SysCheckPassword",
            data: { Password: CurrentPassword }
        }).done(function (data) {
            if (data == 0) {


                $.bigBox({
                    title: "提示",
                    content: "<i class='fa fa-check'></i> <i>出错了,密码出错！</i>",
                    color: "#C46A69",

                    icon: "fa  fa-warning",

                    timeout: 2000
                });




               // $("#message").text("当前密码错误！请重新输入");
                $("#CurrentPassword").val("");
                return;
            }
            else
                $("#message").text("");
        })
    });
    //提交时验证新密码
    $("#confirmBtn").click(function () {
        var NewPassword = $("#NewPassword").val();
        var ConfirmPassword = $("#ConfirmPassword").val();
        var CurrentPassword = $("#CurrentPassword").val();
        if (ConfirmPassword == "" || NewPassword == "" || CurrentPassword == "") {
            alert("请完成输入");
            return;
        }
        if (NewPassword.length < 6 || NewPassword.length > 20) {
            alert("密码长度为6—20位");
            return;
        }
        if (ConfirmPassword != NewPassword) {
            alert("两次输入密码不一致");
            return;
        }
        $.ajax({
            type: "post",
            url: "/WebApi/SysManager/SysUpdatePassword",
            data: { Password: NewPassword }
        }).done(function (data) {
            if (data == 1)
                alert("密码更新成功");
        })


        return false;
    });

    $("#replaceBtn").click(function () {
        $("#NewPassword").val("");
        $("#ConfirmPassword").val("");
        $("#CurrentPassword").val("");
    });
}