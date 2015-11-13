
//左Tree 中tree 
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


var ztSettingMid = {
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
            gdViewModel.zTreeOnClickMid(event, treeId, treeNode);  //点击事件
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



function treeAndTreeModel(options) {
    
    var self = this;


    self.NodeValue = options.NodeValue; //节点数据


    self.UpdateBtn = options.UpdateBtn;
    self.DelBtn = options.DelBtn;
    self.AddBtn = options.AddBtn;



    //CRUD均通过ajax调用实现，这里提供用于获取ajax请求地址的方法
    self.dataQueryUrlAccessor = options.dataQueryUrlAccessor;
    self.dataAddUrlAccessor = options.dataAddUrlAccessor;
    self.dataUpdateAccessor = options.dataUpdateAccessor;
    self.dataDeleteAccessor = options.dataDeleteAccessor;

    self.dataNodeSelect = options.dataNodeSelect; //选中节点的url
    self.hidId = options.hidId; //隐藏id框
    self.frmElement = options.frmElement;//需要验证的表单


    self.renderDiv = options.renderDiv;
    self.renderDivLeft = options.renderDivLeft;
    //removeData：删除操作完成后将数据从recordSet中移除
    //replaceData：修改操作后更新recordSet中相应记录
    self.removeData = options.removeData;
    self.replaceData = options.replaceData;

    self.highLighNodeId = 0;//高亮结点id

    //***********************************left tree
    self.dataLeftTreeQueryUrl = options.dataLeftTreeQueryUrl;//左边的tree查询

    self.SelectNodeIDLeft = options.SelectNodeIDLeft; //隐藏被选中节点框
    self.renderDivLeft = options.renderDivLeft;//占位图层
  
    if (options.selectNodeAfter) {
        self.selectNodeLeftAfter = options.selectLeftNodeAfter;//节点选中后置
    }
    self.highLighNodeIdLeft = 0;//高亮结点id
    self.hidIdLeft = options.hidIdLeft; //隐藏id框,左树
    /***************end left tree **********************/
    //初始化
    self.init = function () {
        self.treeinit();

    }

    //初始化
    self.treeinit = function () {

        $.ajax(
       {
           url: self.dataLeftTreeQueryUrl(self)
           , type: "GET"
           , success: function (data, text) {
               self.zTreeDivLeft = $.fn.zTree.init(self.renderDivLeft, ztSetting, data);


               var selNode = self.zTreeDivLeft.getNodeByParam("id", self.highLighNodeIdLeft, null); //如果有高亮节点，则选中它
               self.zTreeDivLeft.selectNode(selNode);



               self.SelectNodeIDLeft.val(self.highLighNodeId);
               self.midTreeShow();//显示对应的中间树数据


           }
           , error: function () {
               alert("获取相关树数据出错！");
           }
       });

    };

    //中间树查询
    self.midTreeShow = function () {
       
        $.ajax(
        {
            url: self.dataQueryUrlAccessor(self),
            type: "GET",
            success: function (data, text) {
                self.zTreeDiv = $.fn.zTree.init(self.renderDiv, ztSettingMid, data);


                var selNode = self.zTreeDiv.getNodeByParam("id", self.highLighNodeId, null); //如果有高度节点，则选中它
                self.zTreeDiv.selectNode(selNode);

                if (self.highLighNodeId != 0 || self.highLighNodeId != "0") {
                    self.showDetail(self.highLighNodeId);
                }

            }
        });
    };


    //选中结点事件,左树
    self.zTreeOnClick = function (event, treeId, treeNode) {

        self.SelectNodeIDLeft.val(treeNode.id); //保存选择id

        if (treeNode.id != 0) {

            self.SelectNodeIDLeft.val(treeNode.id);
           
            self.midTreeShow();//显示对应的中树

            if (self.selectNodeAfterLeft) {
                self.selectNodeAfterLeft(treeNode);//节点选中后置
            }
        } else {

            $.globalMessenger().post({ message: "请选择非根结点查看对应的数据！", hideAfter: 3, type: 'error' });
        }
    };

    //选中结点事件,中树
    self.zTreeOnClickMid = function (event, treeId, treeNode) {

        self.hidId.val(treeNode.id); //保存选择id

        if (treeNode.id != 0) {

            self.showDetail(treeNode.id);

        }
    };

    //显示结点详细信息
    self.showDetail = function (selectNodeId) {
        //如果不是根,则需要取数据
        $.ajax(
        {
            url: self.dataNodeSelect(selectNodeId)
            , type: "GET"
            , success: function (data) {
                self.NodeValue(data);

            }
            ,
            error: function () {
                alert("获取相关节点数据出错！可能是服务器无反应!!");
            }
        });
    }





    //保存更新事件
    self.updateNode = function () {
        //(1)读取更新

        if (self.frmElement.valid()) {

            self.UpdateBtn.attr('disabled', 'disabled');
            self.DelBtn.attr('disabled', 'disabled');
            self.AddBtn.attr('disabled', 'disabled');

            var jsonData = JSON.stringify(self.NodeValue());
            $.ajax(
           {
               url: self.dataUpdateAccessor(self.hidId.val())
               , type: "PUT"
               , contentType: "application/json; charset=utf-8"

               , data: jsonData
               , success: function (data, text) {

                   $.globalMessenger().post({ message: "修改信息成功！", hideAfter: 3, type: 'success' });
                   self.highLighNodeId = self.hidId.val();
                   self.zTreeDiv.destroy();
                   self.midTreeShow();

               }


           }).always(function () {
               self.UpdateBtn.removeAttr("disabled");
               self.DelBtn.removeAttr("disabled");
               self.AddBtn.removeAttr("disabled");

           });

        }


    };


    //保存新建事件.AddNewNode
    self.AddNewNode = function () {

        if ($.trim(self.hidId.val()) == "" && $.trim(self.SelectNodeIDLeft.val()) == "") {
            alert("出错了，请先选中一个节点，再为它加上子节点！！注意：左边的树一定要选中!!");
            return;
        };

        self.UpdateBtn.attr('disabled', 'disabled');
        self.DelBtn.attr('disabled', 'disabled');
        self.AddBtn.attr('disabled', 'disabled');


        $.ajax(
       {
           url: self.dataAddUrlAccessor(self.hidId.val())
           , type: "Post"
           , contentType: "application/json; charset=utf-8"

           , success: function (data, text) {

               $.globalMessenger().post({ message: "新增结点成功！", hideAfter: 3, type: 'success' });
               self.highLighNodeId = data;
               self.hidId.val(data);

               self.zTreeDiv.destroy();
               self.midTreeShow();



           }

       }).always(function () {
           self.UpdateBtn.removeAttr("disabled");
           self.DelBtn.removeAttr("disabled");
           self.AddBtn.removeAttr("disabled");

       });


    };


    //删除结点
    self.DelNode = function () {

        if (!confirm("是否真的要删除结点？此过程不可恢复!")) {
            return;
        }
        if ($.trim(self.hidId.val()) == "") {
            alert("出错了，请先选中一个节点，再选择删除！！");
            return;
        } else if (self.hidId.val() == "0") {
            alert("出错了，无法删除根结点！！");
            return;
        }


        self.UpdateBtn.attr('disabled', 'disabled');
        self.DelBtn.attr('disabled', 'disabled');
        self.AddBtn.attr('disabled', 'disabled');
        $.ajax(
       {
           url: self.dataDeleteAccessor(self.hidId.val())
           , type: "DELETE"

           , success: function (data, text) {

               $.globalMessenger().post({ message: "删除结点成功！", hideAfter: 3, type: 'success' });
               self.highLighNodeId = data;
               self.hidId.val(data);
               self.zTreeDiv.destroy();
               self.midTreeShow();



           }
           , error: function (xhr, textStatus, errorThrown) {

               if (xhr.status == 409) {
                   alert("为安全，无法直接删除有子结点的树！！");
               } else {
                   alert("出错了！");
               }

           }

       }).always(function () {
           self.UpdateBtn.removeAttr("disabled");
           self.DelBtn.removeAttr("disabled");
           self.AddBtn.removeAttr("disabled");

       });

    }




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


