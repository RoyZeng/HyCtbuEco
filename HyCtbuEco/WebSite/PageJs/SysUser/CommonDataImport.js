
function CommonDataImportRunPage() {

    var tempID;//保存当前的选项的Id;
    //var gdViewModel;

    var IsError; //1表示出错binddata
    var IsClick = '';//1表示提交过数据
    var checkVal = "";
    
    //向导初始化
    $("#ShowDownLoadID").hide();
    $('#bootstrap-wizard-1').bootstrapWizard({
        'tabClass': 'form-wizard',
        onTabClick: function (tab, navigation, index) {
            return false;
        },
        //点击“上一步”
        'onPrevious': function (tab, navigation, index) {
            var $total = navigation.find('li').length;
            var $current = index + 1;//预获取下一个Index
            if ($current >= $total) {
                $('#bootstrap-wizard-1').find('.pager .next').hide();
                $('#bootstrap-wizard-1').find('.pager .finish').show();
                $('#bootstrap-wizard-1').find('.pager .finish').removeClass('disabled');
            } else {
                $('#bootstrap-wizard-1').find('.pager .next').show();
                $('#bootstrap-wizard-1').find('.pager .finish').hide();

                $("#ShowDownLoadID").hide();
            }
        },
        //点击“下一步”
        'onNext': function (tab, navigation, index) {


            var $total = navigation.find('li').length;
            var $current = index + 1;//预获取下一个Index
            if ($current < $total) {
                $('#bootstrap-wizard-1').find('.pager .next').show();
                $('#bootstrap-wizard-1').find('.pager .finish').hide();

            }

            if (index == 1) {
                confirm("你确定使用当前的数据库表吗？")  //确认信息test




            }
            else if (index == 2) {
                
                var uploader = WebUploader.create({
                    // 选完文件后，是否自动上传。
                    auto: true,
                    // swf文件路径
                    swf: '/Scripts/webuploader/Uploader.swf',
                    // 文件接收服务端。
                    server: '/WebApi/CommonDataImport/btnImportDataClick/',
                    // 选择文件的按钮。可选。
                    // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                    pick: '#filePicker',
                    //文件上传请求的参数表
                    formData: {
                        tableName: $("#tableName").val(),
                        cityCode: $("#cityName").val(),
                        attrArr: $("form[name='selAttr']").serialize()
                    },

                    //验证单个文件大小。10M   1024*1024*10
                    fileSingleSizeLimit: 10485760,

                    // 只允许选择图片文件。
                    //accept: {
                    //    title: 'Images',
                    //    extensions: 'gif,jpg,jpeg,bmp,png',
                    //    mimeTypes: 'image/*'
                    //}
                });


                // 上传失败
                uploader.on('uploadError', function (file, response) {
                    alert(response);
                });
                // 上传成功
                uploader.on('uploadSuccess', function (file, response) {
                    alert(response);
                });

                $("#hrefID").removeClass("disabled");

            }
            else if (index == 3) {

                alert(index);
            }
            else if (index == 4) {


            }

            $('#bootstrap-wizard-1').find('.form-wizard').children('li').eq(index - 1).addClass(
              'complete');
            $('#bootstrap-wizard-1').find('.form-wizard').children('li').eq(index - 1).find('.step')
            .html('<i class="fa fa-check"></i>');
        }
    });

    


    options = {

        renderDiv: $("#DimIndCateTree"),
        hidId: $("#hidId"),
        frmElement: $("#frmDimIndCate"),



        UpdateBtn: $("#btnDimIndCateUpdate"),
        AddBtn: $("#btnDimIndCateAdd"),
        DelBtn: $("#btnDimIndCateDel"),


        //节点数据
        NodeValue: ko.observable({

            Id: 0, SCateName: '', ILibID: 0, IParentID: 0, ILevel: 0, ILeaf: 0, SCateIntro: '', SCateAllName: '', SMemo: '', SCateCode: ''

        }),

        //用户查询URL
        dataQueryUrlAccessor: function () { return "/WebApi/DimIndCate/TreeAll"; },

        //添加用户URL
        dataAddUrlAccessor: function (key) { return ""; },
        dataUpdateAccessor: function (key) { return ""; },
        dataDeleteAccessor: function (key) { return ""; },
        dataNodeSelect: function (id) { return ""; }
    }

    viewModel = new ViewModel(options);
    //重写显示节点内容
    viewModel.showDetail = function (selectNodeId) {
        $.ajax(
        {
            url: "/WebApi/CommonDataImport/GetIndicator?cateId=" + selectNodeId
            , type: "GET"
            , success: function (data) {
                var div1 = $("#checkOne"), div2 = $("#checkTwo");
                div1.empty(); div2.empty();
                var htmlStr = "";
                
                for (var i = 0; i < data.length; i++) {
                    htmlStr = "<label class='checkbox'><input type='checkbox' name='checkZb' value='" + data[i].SIndCode + "'><i></i>"
                    + data[i].SIndName + "</label>";
                    if (i%2===0) {
                        div1.append(htmlStr);
                    }
                    else {
                        div2.append(htmlStr);
                    }
                }
            }
            ,
            error: function () {
                alert("获取相关节点数据出错！可能是服务器无反应!!");
            }
        });
    }


    viewModel.init();
    ko.applyBindings(viewModel, c1);

};

