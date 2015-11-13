
//左Tree右grid 模式
var ztSetting = {
    view: {
        showIcon: true
    },
    check: {
        enable: true,
        chkboxType: { "Y": "s", "N": "" }  //勾选时，父关联子
    },
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        onClick: function (event, treeId, treeNode) {
            gdViewModel.zTreeOnClick(event, treeId, treeNode);  //点击事件
        }
    }
};


//组装访问路径
function appendQueryString(url, parameters) {
    url += "?"
    for (var key in parameters) {
        if (parameters[key] === null || parameters[key] === undefined || parameters[key] === "") {
            continue;
        }
        url += key + "=" + encodeURIComponent(parameters[key]) + "&";
    }
    url = url.substring(0, url.length - 1);
    return url;
}


//判断一个对象是否为空
function isEmptyObject(obj) {
    for (var name in obj) {
        return false;
    }
    return true;
}

function changeSearchToField(searchFiled) {
    //将查询字段转为真实的字段名
    //示例：txtbSrSName -->S_Name
    //也可能把SName-->S_Name;
    if (searchFiled.toLowerCase() == "id")
        return id;//id不转换
    var tmpIndex = searchFiled.indexOf('Sr');//查找第一个Sr出现的位置
    if (tmpIndex > 0) {
        var tmpStr = searchFiled.substring(tmpIndex + 2, tmpIndex + 3) + "_" + searchFiled.substring(tmpIndex + 3);
    } else {
        var tmpStr = searchFiled.substring(0, 1) + "_" + searchFiled.substring(1);
    }

    return tmpStr;
}


function treeAndGridViewModel(options) {
    var self = this;


    self.tableMaskElement = options.tableMaskElement;   //需要mask的table ID  by zjb


    //标题、数据集、弹出对话框和内容（HTML）

    self.recordSet = ko.observableArray(); //数据集
    self.dialogContent = ko.observable();  //对话框内容
    self.dialog = options.dialogId ? $("#" + options.dialogId) : $("#dialog");   //添加对话框
    self.dataNodeSelect = options.dataNodeSelect; //选中节点的url,对应详细信息

    self.frmElement = options.frmElement;//需要验证的表单

    self.updateDialog = options.updateDialogId ? $("#" + options.updateDialogId) : $("#dialog");  //更改 对话框
    self.pageSize = options.pageSize;  //分页数
    self.totalCount = ko.observable(); //数据总条数


    //排序
    self.orderBy = ko.observable();  //排序字段名
    self.isAsc = ko.observable();  //升序/降序
    self.defaultOrderBy = options.defaultOrderBy;  //默认排序字段

    //分页
    self.totalPages = ko.observable();  //总页数
    self.pageNumbers = ko.observableArray();  //页码列表
    self.pageIndex = ko.observable();  //当前页

    //查询条件：标签和输入值
    self.NodeValue = options.NodeValue;//结点数据，详细
    self.hidId = options.hidId; //隐藏id框
    self.SelectNodeID = options.SelectNodeID; //隐藏被选中节点框


    //作为显示数据的表格的头部：显示文字和对应的字段名（辅助排序）
    self.headers = ko.observableArray(options.headers);

    //CRUD均通过ajax调用实现，这里提供用于获取ajax请求地址的方法
    self.dataQueryUrlAccessor = options.dataQueryUrlAccessor;
    self.dataAddUrlAccessor = options.dataAddUrlAccessor;
    self.dataUpdateAccessor = options.dataUpdateAccessor;
    self.dataDeleteAccessor = options.dataDeleteAccessor;


    self.dataTreeQueryUrl = options.dataTreeQueryUrl;//左边的tree查询

    self.SaveUpdateBtn = options.SaveUpdateBtn;
    self.renderDiv = options.renderDiv;//占位图层
    self.getQueryFullUrl = options.getQueryFullUrl;//grid的查询条件需要变化
    if (options.showDlgBefore){
        self.showDlgBefore = options.showDlgBefore;//对话框打开后,对话框信息显示完成后
    }
    if (options.selectNodeAfter) {
        self.selectNodeAfter = options.selectNodeAfter;//节点选中后置
    }
    //初始化
    self.init = function () {
        self.treeinit();
        
    }
    
    //初始化
    self.treeinit = function () {

        $.ajax(
       {
           url: self.dataTreeQueryUrl(self)
           , type: "GET"
           , success: function (data, text) {
               self.zTreeDiv = $.fn.zTree.init(self.renderDiv, ztSetting, data);


               var selNode = self.zTreeDiv.getNodeByParam("id", self.highLighNodeId, null); //如果有高度节点，则选中它
               self.zTreeDiv.selectNode(selNode);

              

                   self.SelectNodeID.val(self.highLighNodeId);
                   self.search();//显示对应的grid数据
              
                  
           }
           , error: function () {
               alert("获取相关树数据出错！");
           }
       });

    };


    //获取数据  by zjb
    self.dataGet = function () {
        //如果table的ID存在，在获取数据之前添加遮罩
        if (self.tableMaskElement) {
            self.tableMaskElement.mask("loading");
        };

        $.ajax(
    {
        url: self.getQueryFullUrl(self),
        type: "GET",
        complete: function () {  //当数据加载完成，取消遮罩
            if (self.tableMaskElement) {
                self.tableMaskElement.unmask();
            };
        },
        success: function (result) {
            self.recordSet(result.Data);
        }
    });
    }

    //Search按钮,得到查询结果
    self.search = function () {
        self.orderBy(self.defaultOrderBy);
        self.isAsc(false);
        self.pageIndex(1);

        //如果table的ID存在，在获取数据之前添加遮罩 by zjb
        if (self.tableMaskElement) {
            self.tableMaskElement.mask("loading");
        };

        $.ajax(
        {
            url: self.getQueryFullUrl(self),
            type: "GET",
            complete: function () {  //当数据加载完成，取消遮罩  by zjb
                if (self.tableMaskElement) {
                    self.tableMaskElement.unmask();
                };
            },
            success: function (result) {
                self.recordSet(result.Data);
                self.totalPages(Math.ceil(result.DataCount / self.pageSize));   //数据总页数
                self.resetPageNumbders();

                //---totalCount赋值 by zjb 
                self.totalCount(result.DataCount);
                //---totalCount赋值 by zjb  end

                checkLinkForTable();//check
            }
        });
    };

   
    //Reset按钮
    self.reset = function () {
        for (var i = 0; i < self.searchCriteria().length; i++) {
            self.searchCriteria()[i].value("");
        }
    };

    //获取数据之后根据记录数重置页码
    self.resetPageNumbders = function () {
        self.pageNumbers.removeAll(); //最多显示10条
        var first = parseInt(self.pageIndex() / self.pageSize) + 1; //起始,从1开始
        var last = first + 10;//结束
        if (last > self.totalPages()) {
            last = self.totalPages();
        }
        for (var i = first; i <= last; i++) {
            self.pageNumbers.push(i);
        }
    };

    //点击表格头部进行排序
    self.sort = function (header) {
        var trueField = changeSearchToField(header.value);
        if (self.orderBy() == trueField) {
            self.isAsc(!self.isAsc());
        }

        self.orderBy(trueField);
        self.pageIndex(1);
        $.ajax(
        {
            url: self.getQueryFullUrl(self),
            type: "GET",
            success: function (result) {
                self.recordSet(result.Data);
            }
        });
    };

    //点击页码获取当前页数据
    self.turnPage = function (pageIndex) {
        self.pageIndex(pageIndex);
        $.ajax(
        {
            url: self.getQueryFullUrl(self),
            type: "GET",
            success: function (result) {
                self.recordSet(result.Data);
            }
        });
    };


    //下一页 by zjb
    self.nextPage = function () {
        var curPage = self.pageIndex();
        if (curPage != self.totalPages()) {

            self.pageIndex(curPage + 1);
            self.dataGet();
        }
    };

    //上一页  by zjb
    self.prevPage = function () {
        var curPage = self.pageIndex();
        if (curPage != 1) {
            self.pageIndex(curPage - 1);
            self.dataGet();
        }
    };

    //第一页 by zjb
    self.firstPage = function () {
        self.pageIndex(1);
        self.dataGet();
    }

    //最后一页 by zjb
    self.lastPage = function () {
        var curPage = self.totalPages();
        self.pageIndex(curPage);
        self.dataGet();

    }


    //输入页码获取该页数据  by zjb
    self.changePageIndex = function () {
        self.dataGet();

    };





    //点击"添加用户"按钮弹出“添加用户”对话框
    self.showAddBefore = function (data) {
        self.hidId.val(0);//新增,id为0


        self.dialog.modal("show");
        var seledted = $(':input', self.frmElement)
             .not(':button, :submit, :reset');
        seledted = seledted.not(self.SelectNodeID);//移除不能清空的元素
        seledted.val('')
           .removeAttr('checked')
           .removeAttr('selected');

        if (self.showDlgBefore) {
            self.showDlgBefore();
        }
    };

    //点击“编辑”，弹出编辑对话框
    self.showUpdateModal = function (currUser) {
        var key = currUser.Id;
        self.hidId.val(key);//修改
        self.dialog.modal("show");
        $(':input', self.frmElement)
        .not(':button, :submit, :reset')
        .val('')
        .removeAttr('checked')
        .removeAttr('selected');


        self.showDetail(key);//显示信息

    };


    //显示结点详细信息
    self.showDetail = function (selectNodeId) {
        //点击显示的时候，需要查看数据
        $.ajax(
        {
            url: self.dataNodeSelect(selectNodeId)
            , type: "GET"
            , success: function (data) {
                self.NodeValue(data);

            }
            , error: function () {
                alert("获取相关数据出错！可能是服务器无反应!!");
            }
        });
    }


    //点击对话框save按钮，保存用户数据
    self.onDataAddingOrUpdate = function () {
        //输入验证，验证成功，则传入后台

        if (self.frmElement.valid()) {
            //根据不同的hidID，决定应当是新增还是更新
            var jsonData = JSON.stringify(self.NodeValue());
            if (self.hidId.val() == 0 || self.hidId.val() == "0") {


                self.SaveUpdateBtn.attr('disabled', 'disabled');



                $.ajax(
               {
                   url: self.dataAddUrlAccessor()
                   , type: "Post"
                   , contentType: "application/json; charset=utf-8"
                    , data: jsonData
                   , success: function (data, text) {

                       $.globalMessenger().post({ message: "新增数据成功！", hideAfter: 3, type: 'success' });
                       self.highLighNodeId = data;
                       self.hidId.val(data);


                       
                       self.search();//重刷新grid



                   }

               }).always(function () {
                   self.SaveUpdateBtn.removeAttr("disabled");
                   self.dialog.modal("hide");

               });


            } else {
                //更新数据
                self.SaveUpdateBtn.attr('disabled', 'disabled');



                $.ajax(
               {
                   url: self.dataUpdateAccessor(self.hidId.val())
                   , type: "PUT"
                   , contentType: "application/json; charset=utf-8"

                   , data: jsonData


                   , success: function (data, text) {

                       $.globalMessenger().post({ message: "更新数据成功！", hideAfter: 3, type: 'success' });
                       self.highLighNodeId = data;
                       self.hidId.val(data);


                       self.search();//重刷新grid



                   }

               }).always(function () {
                   self.SaveUpdateBtn.removeAttr("disabled");
                   self.dialog.modal("hide");

               });

            }


        }
    };




    //点击Delete按钮删除当前记录
    self.onDataDeleting = function (data) {
        if (confirm("确定要删除数据吗？")) {
            var tmpUrl = self.dataDeleteAccessor(data, self);
            if (tmpUrl == "") {
                alert("请先勾选被删除的数据，再删除！！");
                return false;
            }
            $.ajax(
            {
                url: tmpUrl,
                type: "DELETE",
                statusCode: {
                    200: function (result) {
                        $.globalMessenger().post({ message: "删除数据成功！", hideAfter: 3, type: 'success' });



                        self.search();//重刷新grid
                    },
                    400: function () {
                        alert("后台删除用户出错！");
                    }
                }
            });
        }
    };


    //选中结点事件
    self.zTreeOnClick = function (event, treeId, treeNode) {

        self.hidId.val(treeNode.id); //保存选择id

        if (treeNode.id != 0) {

            self.SelectNodeID.val(treeNode.id);
            self.search();//显示对应的grid数据

            if (self.selectNodeAfter) {
                self.selectNodeAfter(treeNode);//节点选中后置
            }
        } else {
        
            $.globalMessenger().post({ message: "请选择非根结点查看对应的数据！", hideAfter: 3, type: 'error' });
        }
    };

    //TO Add Method

}

//去除多余的分割符，例如：",a",结果为"a"
function dropRsplit(a, splitC) {
    var tmpA = a.split(splitC);
    var tmpObj = new Array();
    for (var i = 0; i < tmpA.length; i++) {
        if (tmpA[i] != "") {
            tmpObj.push(tmpA[i]);
        }

    }
    return tmpObj.join(splitC);
}




//json化对象
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};


var checkLinkForTable = function () {
    $('.checkboxs thead :checkbox').change(function () {
        if ($(this).is(':checked')) {
            $('.checkboxs tbody :checkbox').prop('checked', true).parent().addClass('checked');
            $('.checkboxs tbody tr.selectable').addClass('selected');
            $('.checkboxs_actions').show();
        }
        else {
            $('.checkboxs tbody :checkbox').prop('checked', false).parent().removeClass('checked');
            $('.checkboxs tbody tr.selectable').removeClass('selected');
            $('.checkboxs_actions').hide();
        }
    });
}