
//开始SysLog.js 脚本
function SysLogRunPage() {


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


    options = {

        dialogId: "SysLogMessageModal", //添加对话框

        pageSize: 15, //分页大小
        hidId: $("#txtbID"),
        SaveUpdateBtn: $("#btnSaveOrUpdateModel"),
        frmElement: $("#formSysLog"),

        //节点数据
        NodeValue: ko.observable({

            Id: 0,
            ILogType: 0,
            SMessage: '',
            IUserID: 0,
            DWriteTime: new Date(),
            SMemo: '',


        }),


        //用户列表Grid头部列名称
        headers: [


                     { displayText: 'Id', value: 'Id', width: 'auto' },
{ displayText: '日志类型', value: 'ILogType', width: 'auto' },
{ displayText: '标题', value: 'SMessage', width: 'auto' },
{ displayText: '用户ID', value: 'IUserID', width: 'auto' },
{ displayText: '写入时间', value: 'DWriteTime', width: 'auto' }



        ],

        //默认的排序方式
        defaultOrderBy: "Id",
        //用户查询URL
        dataQueryUrlAccessor: function () {
            return "/WebApi/SysLog/GetByPage";
        },
        AddBefore: function () {
            //表单发送前的函数
            return true;
        },

        //添加用户URL
        dataAddUrlAccessor: function () { return "/WebApi/SysLog/Post"; },
        dataUpdateAccessor: function (key) { return "/WebApi/SysLog/Put/" + key; },
        dataNodeSelect: function (id) {
            return "/WebApi/SysLog/get/" + id;
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
                return appendQueryString("/WebApi/SysLog/Delete", { ids: userIDs });
            } else
                return "";

        }
        , initEd: function () {
            //gird初始化的后置事件 信息

        }
        , changeEnd: function () {
            //对话框关闭后 的后置事件
        }
        , showDetailEnd: function (data) {
            //显示对话框详细之后
        }, showDlgBefore: function () {
            //对话框打开后，对话框信息显示完成后
        }



    };

    gdViewModel = new gridViewModel(options);


    gdViewModel.init();
    ko.applyBindings(gdViewModel, c1);


    options.frmElement.validate({
        rules: {


            txtbID: { required: true, digits: true }, txtbILogType: { digits: true }, txtbSMessage: { maxlength: 50 }, txtbIUserID: { digits: true }, txtbDWriteTime: { date: true }, txtbSMemo: { maxlength: 50 }



        },
        messages: {


            txtbID: { required: '必填项', digits: '必须是整数' }, txtbILogType: { digits: '必须是整数' }, txtbSMessage: { maxlength: '最大长度为2000' }, txtbIUserID: { digits: '必须是整数' }, txtbDWriteTime: { date: '必须是日期格式，例如：2013/01/01' }, txtbSMemo: { maxlength: '最大长度为100' }

        }
    });


};
