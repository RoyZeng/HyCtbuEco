function RunPage() {
    var ue = new baidu.editor.ui.Editor();
    ue.render("editor");//初始化编辑器

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

        dialogId: "NoticesMessageModal", //添加对话框

        pageSize: 15, //分页大小
        hidId: $("#txtbID"),
        SaveUpdateBtn: $("#btnSaveOrUpdateModel"),
        frmElement: $("#formNotices"),

        //节点数据
        NodeValue: ko.observable({

            Id: 0,
            SSenderNo: '',
            SSenderName: '',
            STitle: '',
            DSendtime: new Date(),
            INoticesType: 0,
            SSendIp: '',
            SContent:'',

        }),


        //用户列表Grid头部列名称
        headers: [

{ displayText: '发送者姓名', value: 'SSenderName', width: 'auto' },
{ displayText: '发送标题', value: 'STitle', width: 'auto' },
{ displayText: '发送时间', value: 'DSendtime', width: 'auto' },
{ displayText: '公告类型', value: 'INoticesType', width: 'auto' },

        ],

        //默认的排序方式
        defaultOrderBy: "Id",
        //用户查询URL
        dataQueryUrlAccessor: function () {
            return "/WebApi/Notices/GetByPage";
        },

        //添加用户URL
        dataAddUrlAccessor: function () {
            //保存Ext信息
            var postData = ue.getContent();
            options.NodeValue().SContent = postData;

            return "/WebApi/NoticesExt/CreateNotices";
        },
        dataUpdateAccessor: function (key) {        
            //保存Ext信息
            var postData = ue.getContent();
            options.NodeValue().SContent = postData;

            return "/WebApi/NoticesExt/Update/" + key;
        },
        dataNodeSelect: function (id) {
            return "/WebApi/NoticesExt/GetNotices/" + id;
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
                return appendQueryString("/WebApi/NoticesExt/DeleteNotices", { ids: userIDs });
            } else
                return "";

        }
        , initEd: function () {
            //gird初始化的后置事件 信息

        }
        , showDlgBefore: function () {
            ue.setContent("");//清空ue
        }
        , showDetailEnd: function (data) {

            ue.setContent(options.NodeValue().SContent);//显示ue
        }
        , changeEnd: function (key, event) {
            //对话框保存的后置事件
        }

    };

    gdViewModel = new gridViewModel(options);


    gdViewModel.init();

    //公告类型
    gdViewModel.NoticesType = ko.observableArray([
                new Tms.optionItem("内部人员", 1),
                new Tms.optionItem("用户", 2),
                new Tms.optionItem("供应商", 3),
    ]);

    ko.applyBindings(gdViewModel, c1);


    options.frmElement.validate({
        rules: {          
            //txtbSTitle: { maxlength: 50 },          
        },
        messages: {           
            //txtbSTitle: { maxlength: '最大长度为100' },
        }
    });
   
};

//公告类型显示转换
function ChangeNoticesType(value) {
    if (value == 1) {
        return "内部人员";
    }
    else if (value == 2) {
        return "用户";
    }
    else if (value == 3) {
        return "供应商";
    }
    else {
        return "";
    }
}