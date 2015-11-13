
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

function changeSearchToField(searchFiled) {   //修正函数，不再为字段加上"_"  修改于1.11日
    //将查询字段转为真实的字段名
    //示例：txtbSrSName -->S_Name
    //也可能把SName-->S_Name;
    if (searchFiled.toLowerCase() == "id")
        return searchFiled;//id不转换
    var tmpIndex = searchFiled.indexOf('Sr');//查找第一个Sr出现的位置
    if (tmpIndex > 0) {
        var tmpStr = searchFiled.substring(tmpIndex + 2, tmpIndex + 3) + searchFiled.substring(tmpIndex + 3);
    } else {
        var tmpStr = searchFiled.substring(0, 1) + searchFiled.substring(1);
    }

    return tmpStr;

}

//字符串转日期格式，strDate要转为日期格式的字符串
function getDate(strDate) {
    var newStr = strDate.replace(/\d+(?=-[^-]+$)/,
     function (a) { return parseInt(a, 10) - 1; }).match(/\d+/g);
    if (newStr[1]) {
        newStr[1] = newStr[1] - 1;//月分不对，要减一
    }
    var date = eval('new Date(' + newStr + ')');
    return date;
}
function gridViewModel(options) {
    var self = this;


    self.DefultGet = "GET";
    if (typeof (options.DefultGet) != "undefined" && options.DefultGet != "") {

        self.DefultGet = options.DefultGet;
    }

    //标题、数据集、弹出对话框和内容（HTML）
    self.tableMaskElement = options.tableMaskElement;   //需要mask的table ID  by zjb

    self.recordSet = ko.observableArray(); //数据集
    self.dialogContent = ko.observable();  //对话框内容
    self.dialog = options.dialogId ? $("#" + options.dialogId) : $("#dialog");   //添加对话框
    self.dataNodeSelect = options.dataNodeSelect; //选中节点的url,对应详细信息

    self.frmElement = options.frmElement;//需要验证的表单

    self.updateDialog = options.updateDialogId ? $("#" + options.updateDialogId) : $("#dialog");  //更改 对话框
    self.pageSize = options.pageSize;  //分页数
    self.totalCount = ko.observable(); //数据总条数
    self.dataLast = ko.observable();//结束数据索引
    self.dataIndex = ko.observable();//开始数据索引


    //排序
    self.orderBy = ko.observable();  //排序字段名
    self.isAsc = ko.observable(false);  //升序/降序
    self.defaultOrderBy = options.defaultOrderBy;  //默认排序字段

    //分页
    self.totalPages = ko.observable();  //总页数
    self.pageNumbers = ko.observableArray();  //页码列表
    self.pageIndex = ko.observable();  //当前页

    //查询条件：标签和输入值
    self.NodeValue = options.NodeValue;//结点数据，详细
    self.hidId = options.hidId; //隐藏id框


    //作为显示数据的表格的头部：显示文字和对应的字段名（辅助排序）
    self.headers = ko.observableArray(options.headers);

    //CRUD均通过ajax调用实现，这里提供用于获取ajax请求地址的方法
    self.dataQueryUrlAccessor = options.dataQueryUrlAccessor;
    self.dataAddUrlAccessor = options.dataAddUrlAccessor;
    self.dataUpdateAccessor = options.dataUpdateAccessor;
    self.dataDeleteAccessor = options.dataDeleteAccessor;

    self.SaveUpdateBtn = options.SaveUpdateBtn;

    self.showDlgBefore = options.showDlgBefore;//对话框打开后,对话框信息显示完成后
    self.showDetailEnd = options.showDetailEnd;//显示对话框详细之后


    self.changeEnd = options.changeEnd;//对话框关闭后

    self.initEd = options.initEd;//grid显示完成后

    //发送更新前 by lx 2015.1.24
    if (options.UpdateBefore) {
        self.UpdateBefore = options.UpdateBefore;
    };

    if (options.isAsc) {

        self.isAsc(options.isAsc);
    }
    if (options.AddBefore) {
        self.AddBefore = options.AddBefore;

    };//前注入
    //处理data前注入
    if (options.beforeData) {
        self.beforeData = options.beforeData;
    }
    if (options.myAjaxComp) {
        //ajax search完成
        self.myAjaxComp=options.myAjaxComp;
    }

    //初始化
    self.init = function () {
        self.search();
    }

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

            self.dataIndex((self.pageIndex() - 1) * self.pageSize + 1);
            self.dataLast(self.pageIndex() * self.pageSize);
            if (self.dataLast() >= self.totalCount()) {
                self.dataLast(self.totalCount());
            }

            self.recordSet(result.Data);

            self.initEd(result);
        }
    });
    }



    //Search按钮,得到查询结果
    self.search = function () {
        self.orderBy(self.defaultOrderBy);
        
        self.pageIndex(1);
        //如果table的ID存在，在获取数据之前添加遮罩 by zjb
        if (self.tableMaskElement) {
            self.tableMaskElement.mask("loading");
        };

        if (self.DefultGet == 'GET') {
            $.ajax(
            {

                url: self.getQueryFullUrl(self),
                type: "GET",
                cache: false,
                complete: function () {  //当数据加载完成，取消遮罩  by zjb
                   // console.log("gridcommon中的completecomplete");
                    if (self.myAjaxComp) {
                        self.myAjaxComp();

                    }
                    if (self.tableMaskElement) {
                        self.tableMaskElement.unmask();
                    };

                },
                success: function (result) {
                    var total = 0;
                    if (self.beforeData) {

                        var rsBefore = self.beforeData(result);
                        if(rsBefore){
                            return true;//后面的不再执行
                        }
                    }

                    if (result == null) {
                        self.recordSet([]);
                    } else {
                        self.recordSet(result.Data);
                        total = Math.ceil(result.DataCount / self.pageSize);


                        if (total >= 1) {
                            self.totalPages(total);
                        } else { self.totalPages(1); }

                        self.resetPageNumbders();


                        //---totalCount赋值 by zjb 
                        self.totalCount(result.DataCount);
                        self.dataIndex((self.pageIndex() - 1) * self.pageSize + 1);
                        self.dataLast(self.pageIndex() * self.pageSize);
                        if (self.dataLast() >= self.totalCount()) {
                            self.dataLast(self.totalCount());
                        }

                        //---totalCount赋值 by zjb  end
                    }


                    checkLinkForTable();//check

                    self.initEd(result);
                }
            });//END AJAX

        }//END get
        else {
            //使用POST方式传数据
            var dataObj = $("#formSearch").serializeObject();

            dataObj.firstResult = (gdViewModel.pageIndex() - 1);
            dataObj.pagesize = gdViewModel.pageSize;
            dataObj.orderBy = gdViewModel.orderBy();
            dataObj.ASC = gdViewModel.isAsc();


            $.ajax(
           {

               url: self.dataQueryUrlAccessor(),
               type: "POST",
               cache: false,
               dataType: "JSON",
               data: dataObj,
               complete: function () {  //当数据加载完成，取消遮罩  by zjb
                  // console.log("gridcommon中的completecomplete");
                   if (self.myAjaxComp) {
                       self.myAjaxComp();

                   }
                   if (self.tableMaskElement) {
                       self.tableMaskElement.unmask();
                   };
                   
               },
               success: function (result) {

                   var total = 0;
                   if (self.beforeData) {
                       self.beforeData(result);
                   }

                   if (result == null) {
                       self.recordSet([]);
                   } else {
                       self.recordSet(result.Data);
                       total = Math.ceil(result.DataCount / self.pageSize);

                       if (total >= 1) {
                           self.totalPages(total);
                       } else { self.totalPages(1); }

                       self.resetPageNumbders();


                       //---totalCount赋值 by zjb 
                       self.totalCount(result.DataCount);
                       self.dataIndex((self.pageIndex() - 1) * self.pageSize + 1);
                       self.dataLast(self.pageIndex() * self.pageSize);
                       if (self.dataLast() >= self.totalCount()) {
                           self.dataLast(self.totalCount());
                       }

                       //---totalCount赋值 by zjb  end
                   }

                   checkLinkForTable();//check

                   self.initEd(result);
               }
           });//END AJAX


        }
    };

    self.getQueryFullUrl = function (a, b) {

        var searchObj = $("#formSearch").serializeObject();

        var whereClause = ""; //准备修改查询条件
        var isFirst = true;
        var tmpStr1 = "";

        for (var prop in searchObj) {
            if (searchObj[prop] != "" && searchObj[prop] != -1) {
                //加入条件,注意:-1表示不做条件要求

                var FieldName = changeSearchToField(prop);
                var firstChar = FieldName.substring(0, 1);
                switch (firstChar) {

                    case "S":
                        tmpStr1 = "(" + FieldName + "  like '%" + searchObj[prop] + "%' )";
                        break;
                    case "D"://年代处理

                        tmpStr1 = "(year(" + FieldName + ")  =" + searchObj[prop] + " )";
                        break;
                    default:
                        tmpStr1 = "(" + FieldName + "  =" + searchObj[prop] + ")";
                        break;

                }

                if (isFirst) {

                    whereClause = tmpStr1;

                    isFirst = false;//不再是第一个
                } else {
                    whereClause += " and  " + tmpStr1;
                }

            }
        }

        if (whereClause == "") whereClause = "1=1";//默认条件
        whereClause = encodeURIComponent(whereClause);
        var orderTrueStr = gdViewModel.orderBy();
        if (gdViewModel.isAsc()) {
            orderTrueStr += "  ASC";
        } else {
            orderTrueStr += "  DESC";
        }
        if (b) {
            return appendQueryString("", {
                firstResult: (gdViewModel.pageIndex() - 1),
                pagesize: gdViewModel.pageSize,
                condition: whereClause,
                orderBy: orderTrueStr


            });
        } else {
            return appendQueryString(self.dataQueryUrlAccessor(), {
                firstResult: (gdViewModel.pageIndex() - 1),
                pagesize: gdViewModel.pageSize,
                condition: whereClause,
                orderBy: orderTrueStr


            });
        };
    }

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

        var pagesize = self.pageIndex();
        if (pagesize > self.totalPages()) {
            self.pageIndex(self.totalPages());
        } else if (pagesize <= 0) {
            self.pageIndex(1);
        }
        self.dataGet();

    };



    //点击"添加用户"按钮弹出“添加用户”对话框
    self.showAddBefore = function (data) {
        self.hidId.val(0);//新增,id为0

        self.dialog.modal("show");
        self.showDlgBefore();
        $(':input', self.frmElement)
           .not(':button, :submit, :reset')
           .val('')
           .removeAttr('checked')
           .removeAttr('selected')
           .trigger("change");//触发change事件，通知ko值改变


    };

    //点击“编辑”，弹出编辑对话框
    self.showUpdateModal = function (currUser) {
        var key = currUser.Id;
      
        self.dialog.modal("show");
        $(':input', self.frmElement)
        .not(':button, :submit, :reset')
        .val('')
        .removeAttr('checked')
        .removeAttr('selected');


        self.hidId.val(key);//修改

        self.showDetail(key);//显示信息

    };


    //显示结点详细信息
    self.showDetail = function (selectNodeId) {
        //点击显示的时候，需要查看数据
        $.ajax(
        {
            url: self.dataNodeSelect(selectNodeId)
            , type: "GET"
            , cache: false
            , success: function (data) {

                self.NodeValue(data);

                self.showDetailEnd(data);//修正日历控件的bug,此处做了位置交换，测试

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
            //var jsonData = JSON.stringify( self.NodeValue() );
            if (self.hidId.val() == 0 || self.hidId.val() == "0") {

                if (self.AddBefore) {
                    if (!self.AddBefore(self.NodeValue())) {
                        return;//不能继续执行
                    }
                }

                //self.SaveUpdateBtn.attr( 'disabled', 'disabled' );



                $.ajax(
               {
                   url: self.dataAddUrlAccessor()
                   , type: "POST"
                   , contentType: "application/json; charset=utf-8"
                    , data: JSON.stringify(self.NodeValue())
                   , success: function (data, text) {

                       //$.globalMessenger().post({ message: "新增数据成功！", hideAfter: 3, type: 'success' });
                       self.highLighNodeId = data.Id;


                       self.hidId.val(data.Id);

                       self.changeEnd(self.hidId.val());//后置事件
                       self.init();



                   }

                  , error: function (XMLHttpRequest, textStatus, errorThrown, d) {
                      alert(XMLHttpRequest.responseText);
                  }
               }
              ).always(function () {
                  self.SaveUpdateBtn.removeAttr("disabled");
                  self.dialog.modal("hide");

              });


            } else {
                //更新数据
               
                //更新前操作 by lx 
                if (self.UpdateBefore) {
                    if (!self.UpdateBefore()) {
                        return;//不能继续执行
                    }
                }
                

                self.SaveUpdateBtn.attr('disabled', 'disabled');

                $.ajax(
               {
                   url: self.dataUpdateAccessor(self.hidId.val())
                   , type: "POST"
                   , contentType: "application/json; charset=utf-8"

                   , data: JSON.stringify(self.NodeValue())


                   , success: function (data, text) {

                       //$.globalMessenger().post({ message: "更新数据成功！", hideAfter: 3, type: 'success' });


                       self.changeEnd(self.hidId.val());//后置事件

                       self.init();



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
                type: "POST",
                statusCode: {
                    200: function (result) {
                        //    $.globalMessenger().post({ message: "删除数据成功！", hideAfter: 3, type: 'success' });
                        self.init();
                    },
                    400: function () {
                        alert("后台删除用户出错！");
                    }
                }
            });
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

//checkbox 全选
var checkLinkForTable = function () {
    $('.checkboxs thead :checkbox').change(function () {
        if ($(this).is(':checked')) {
            $('.checkboxs tbody :checkbox').prop('checked', true).parent().addClass('checked');
            // $('.checkboxs tbody tr.selectable').addClass('selected');
            $('.checkboxs_actions').show();
        }
        else {
            $('.checkboxs tbody :checkbox').prop('checked', false).parent().removeClass('checked');
            $('.checkboxs tbody tr.selectable').removeClass('selected');
            $('.checkboxs_actions').hide();
        }
    });
}