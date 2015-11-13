//开始SysUser.js 脚本
function SysUserRunPage() {




    //上级主管
    var txtbSManagerName = $("#txtbSManagerName").select2({
        ajax: {
            url: "/WebApi/SysManager/QueryByUserTrueName",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params,
                    page_limit: 10
                };
            },
            results: function (data) {
                var newdata = new Array();
                for (var i = 0; i < data.length; i++) {
                    var item = new Object();
                    item.id = data[i].Id;
                    item.text = data[i].STrueName;


                    newdata.push(item);
                }
                return { results: newdata };
            }
            , cache: false
        }
        , initSelection: function (element, callback) {

            console.log("element" + element);
            var data = JSON.parse(element.val());
            callback({ id: data.id, text: data.text });//这里初始化
        }
       , minimumInputLength: 1

    });

    //初始化编辑器
    var edit = function () {
        $('.click2edit').summernote({ focus: true });
    };

    //角色字符串的拼接
    var roleStr = function () {
        var check = $(".inline-group input:checked");
        var roleIds = "";
        check.each(function (i) {        //循环拼装被选中项的值
            roleIds = roleIds + ',' + $(this).val();
        });
        roleIds = dropRsplit(roleIds, ",");
        return roleIds;
    }

    //全选的实现
    $("#headcheck").click(function () {
        var x = $('#headcheck').prop("checked");
        if (x == true) {
            $("input[name='userSelector']").each(function () {
                $(this).prop("checked", true);
            });
        }
        else {
            $("input[name='userSelector']").each(function () {
                $(this).prop("checked", false);
            });
        }
    });
    //点击“取消”按钮清空已填内容
    $("#btnCancel").click(function () {
        $("#txtbSPassword").val("");
        $("#confirm_password").val("");

        $("#txtbSOtherPassword").val("");
        $("#confirm_password2").val("");


        $(".note-editable").text("");
        $(".inline-group input:checked").removeAttr("checked");
    });

    //点击“关闭”按钮清空已填内容
    $("#btnClose").click(function () {
        alert("test");
        $("#txtbSPassword").val("");
        $("#confirm_password").val("");


        $("#txtbSOtherPassword").val("");
        $("#confirm_password2").val("");


        $(".note-editable").text("");
        $(".inline-group input:checked").removeAttr("checked");
    });
    edit();
    $(".note-editable").css("height", "300px");  //设置summernote的样式

    options = {

        dialogId: "SysUserMessageModal", //添加对话框

        pageSize: 15, //分页大小
        hidId: $("#txtbID"),
        SaveUpdateBtn: $("#btnSaveOrUpdateModel"),
        frmElement: $("#formSysUser"),

        //节点数据
        NodeValue: ko.observable({

            ID: 0,
            SUserName: '',
            SPassword: '',
            DCreateDate:'',
            DLastLoginDate:'',
            SCIP: '',
            SLIP: '',
            STrueName: '',
            ISex: 0,
            SAddr: '',
            IPart: 0,
            IState: 0,
            SPro: '',
            DBirth:'',
            SNational: '',
            SPhone: '',
            SFax: '',
            SIDCard: '',
            SImg: '',
            SSignIMG: '',
            IScore: 0,
            IPostRecord: 0,
            ILevel: 0,
            SIntroduce: '',
            Sworkplace: '',
            SEmail: '',
            IShowPosition: 0,
            ISort: 0,
            SMem: '',
            SPartName: '',
            SRoleName: '',  //部门名
            SRoleId: '',
            SOtherPassword: '',
            IManagerID: 0,//主管
            SManagerName: '',
            SMainURL:''//主页
        }),


        //用户列表Grid头部列名称
        headers: [
            { displayText: '用户名', value: 'SUserName', width: 'auto' },
            { displayText: '真实姓名', value: 'STrueName', width: 'auto' },
            { displayText: '性别', value: 'ISex', width: 'auto' },
            { displayText: '联系电话', value: 'SPhone', width: 'auto' },
            { displayText: '身份证号', value: 'SIDCard', width: 'auto' },
            { displayText: '邮箱', value: 'SEmail', width: 'auto' },
            { displayText: '最后登录时间', value: 'DLastLoginDate', width: 'auto' },
            { displayText: '所属部门', value: 'SPartName', width: 'auto' },
            { displayText: '拥有的角色', value: 'SRoleName', width: 'auto' },
        ],

        //默认的排序方式
        defaultOrderBy: "ID",
        //用户查询URL
        dataQueryUrlAccessor: function () {
            return "/WebApi/SysManager/GetByPageUserAndUserRole";
        },

        AddBefore: function () {
            //表单发送前的函数
            var p1 = $("#txtbSPassword").val();
            var p2 = $("#confirm_password").val();

            var p3 = $("#txtbSOtherPassword").val();
            var p4 = $("#confirm_password2").val();
            RoleIds = roleStr();
            if (p1 == "" || p2 == "" || p3=="") {
                alert("请填写密码");
                return;
            }
            if (p1.length < 6 || p1.length > 20) {
                alert("密码长度为6—20位");
                return;
            }
            if (p1 != p2 || p3!=p4) {
                alert("两个密码的两次密码输入不一致");
                $("#txtbSPassword").val("");
                $("#confirm_password").val("");
                $("#confirm_password2").val("");
                return;
            }
            if (RoleIds == "") {
                alert("请选择角色！");
                return;
            }
            var birth = $("#txtbDBirth").val();
            options.NodeValue().DBirth = getDate(birth);
            options.NodeValue().SPassword = $("#txtbSPassword").val();
            options.NodeValue().SOtherPassword = p3;

            options.NodeValue().SIntroduce = $(".note-editable").code();
            options.NodeValue().SRoleId = RoleIds;  //拥有的角色对应的id


            //读取上级数据
            var selData2 = txtbSManagerName.select2('data');
            if (selData2 != null) {
                options.NodeValue().IManagerID = selData2.id;
                options.NodeValue().SManagerName = selData2.text;
            }


            return true;
        },

        //添加用户URL
        dataAddUrlAccessor: function () {
            if (UniqueUserName()) {
                return "";
            }
            else {
                return appendQueryString("/WebApi/SysManager/AddUserAndUserRole", { RoleIds: RoleIds });
            }
            
        },
        dataUpdateAccessor: function (key) {
            RoleIds = roleStr();
            var birth = $("#txtbDBirth").val();
            options.NodeValue().DBirth = getDate(birth);

            options.NodeValue().SIntroduce = $(".note-editable").code();
            options.NodeValue().SPassword = $("#txtbSPassword").val();
            options.NodeValue().SOtherPassword = $("#txtbSOtherPassword").val();


            var selData2 = txtbSManagerName.select2('data');
            if (selData2 != null) {
                options.NodeValue().IManagerID = selData2.id;
                options.NodeValue().SManagerName = selData2.text;
            }

            return appendQueryString("/WebApi/SysManager/PutUserAndUserRole/" + key, { RoleIds: RoleIds });

        },
        UpdateBefore: function () {
            //更新前
            var p1 = $("#txtbSPassword").val();
            var p2 = $("#confirm_password").val();

            var p3 = $("#txtbSOtherPassword").val();
            var p4 = $("#confirm_password2").val();
            RoleIds = roleStr();
            if (p1 != "" || p3 != "") {

                if (p1.length < 6 || p1.length > 20) {
                    alert("密码长度为6—20位");
                    return false;
                }
            }
            if (p1 != p2 || p3 != p4) {
                alert("两个密码的两次密码输入不一致");
                $("#txtbSPassword").val("");
                $("#confirm_password").val("");
                $("#confirm_password2").val("");
                return false;
            }
            if (RoleIds == "") {
                alert("请选择角色！");
                return false;
            }

            return true;

        },
        dataNodeSelect: function (id) {
            return "/WebApi/SysManager/GetUserAndUserRole/" + id;
        },

        dataDeleteAccessor: function (data) {
            var userIDs = "";
            var check = $("input[name='userSelector']:checked");  //得到所有被选中的checkbox
            check.each(function (i) {        //循环拼装被选中项的值
                userIDs = userIDs + ',' + $(this).val();
            });
            //清除多余的分割号
            userIDs = dropRsplit(userIDs, ",");
            if (userIDs != "") {
                return appendQueryString("/WebApi/SysManager/DeleteUserAndUserRole", { ids: userIDs });
            } else
                return "";

        }
        , initEd: function () {
            //gird初始化的后置事件 信息

        }, changeEnd: function () {

            //对话框关闭后 的后置事件
            $("#txtbSPassword").val("");
            $("#confirm_password").val("");

            $("#txtbSOtherPassword").val("");
            $("#confirm_password2").val("");


            $(".note-editable").text("");
            $(".inline-group input:checked").removeAttr("checked");
        }, showDetailEnd: function (data) {

            $(".note-editable").prepend(data.SIntroduce);
            //对已拥有的角色进行勾选 
            var roleid = data.SRoleId;
            if (roleid != null && roleid != "") {
                var ids = roleid.split(',');
                for (var i = 0; i < ids.length; i++) {
                    var id = "check" + ids[i];
                    $("#" + id).attr("checked", true);
                }
            }


            var str2 = '{"id":"' + data.IManagerID + '","text":"' + data.SManagerName + '"}';

            txtbSManagerName.select2("val", str2);


            //显示对话框详细之后
        }, showDlgBefore: function () {

        }


    };

    gdViewModel = new gridViewModel(options);

    //性别转换
    gdViewModel.SexType = ko.observableArray([
                new Tms.optionItem("不详", 0),
                new Tms.optionItem("女", 1),
                new Tms.optionItem("男", 2),
    ]);

    gdViewModel.init();
    ko.applyBindings(gdViewModel, c1);


    options.frmElement.validate({
        rules: {
            txtbSUserName: { required: true, maxlength: 25 },
            txtbSPassword: {  maxlength: 250 },
            txtbSOtherPassword: {  maxlength: 250 },
            txtbDCreateDate: { date: true },
            txtbDLastLoginDate: { date: true },
            txtbSCIP: { maxlength: 50 },
            txtbSLIP: { maxlength: 50 },
            txtbSTrueName: { maxlength: 50, required: true },
            txtbISex: { digits: true },
            txtbSAddr: { maxlength: 50 },
            txtbIPart: { required: true, digits: true },
            txtbIState: { digits: true },
            txtbSPro: { maxlength: 50 },
            txtbDBirth: { required: true,date: true },
            txtbSNational: { maxlength: 50 },
            txtbSPhone: { isPhone: true, required: true },
            txtbSFax: { maxlength: 50 },
            txtbSIDCard: { maxlength: 50, required: true },
            txtbSImg: { maxlength: 50 },
            txtbSSignIMG: { maxlength: 50 },
            txtbIScore: { digits: true },
            txtbIPostRecord: { digits: true },
            txtbILevel: { digits: true },
            txtbSIntroduce: { maxlength: 50 },
            txtbSworkplace: { maxlength: 50 },
            txtbSEmail: { email: true, required: true },
            

        },
        messages: {
            txtbSUserName: { required: '必填项', maxlength: '最大长度为25' },
            txtbSPassword: { required: '必填项', maxlength: '最大长度为50' },
            txtbSOtherPassword: { required: '必填项', maxlength: '最大长度为50' },
            txtbDCreateDate: { date: '必须是日期格式，例如：2013/01/01' },
            txtbDLastLoginDate: { date: '必须是日期格式，例如：2013/01/01' },
            txtbSTrueName: { maxlength: '最大长度为250', required: '必填项' },
            txtbISex: { digits: '必须是整数' },
            txtbSAddr: { maxlength: '最大长度为250' },
            txtbIPart: { required: '必填项', digits: '必须是整数' },
            txtbIState: { digits: '必须是整数' },
            txtbSPro: { maxlength: '最大长度为25' },
            txtbDBirth: { date: '必须是日期格式，例如：2013/01/01', required: '必填项' },
            txtbSNational: { maxlength: '最大长度为25' },
            txtbSPhone: { required: '必填项', maxlength: '最大长度为25' },
            txtbSFax: { maxlength: '最大长度为25' },
            txtbSIDCard: { required: '必填项', maxlength: '最大长度为25' },
            txtbSImg: { maxlength: '最大长度为125' },
            txtbSSignIMG: { maxlength: '最大长度为125' },
            txtbIScore: { digits: '必须是整数' },
            txtbIPostRecord: { digits: '必须是整数' },
            txtbSworkplace: { maxlength: '最大长度为50' },
            txtbSEmail: { required: '必填项', maxlength: '最大长度为25' },

        }
    });
};

//验证用户名唯一
function UniqueUserName() {
    var bol = false;
    var username = $("#txtbSUserName").val();  //输入的用户名
    $.ajax({
        url: '/WebApi/SysManager/ValidateUserName',
        type: 'post',
        data: { UserName: username },
        success: function (data) {
            if (data == 1) {
                bol = true;
                alert("该用户名已存在，不可使用");
            }
        }
    });
    return bol;
}