var ztSetting = {
    view: {
        showIcon: true
    },
    //check: {
    //    enable: false,
    //    chkboxType: { "Y": "s", "N": ""}  //勾选时，父关联子
    //},
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        onClick: function (event, treeId, treeNode) {
            viewModel.zTreeOnClick(event, treeId, treeNode);  //点击事件
        }
    }
};
function ViewModel(options) {
    var self = this;

    
    self.NodeValue = options.NodeValue; //节点数据


    self.UpdateBtn = options.UpdateBtn;
    self.DelBtn=options.DelBtn;
    self.AddBtn=options.AddBtn;



    //CRUD均通过ajax调用实现，这里提供用于获取ajax请求地址的方法
    self.dataQueryUrlAccessor = options.dataQueryUrlAccessor;
    self.dataAddUrlAccessor = options.dataAddUrlAccessor;
    self.dataUpdateAccessor = options.dataUpdateAccessor;
    self.dataDeleteAccessor = options.dataDeleteAccessor;

    self.dataNodeSelect = options.dataNodeSelect; //选中节点的url
    self.hidId = options.hidId; //隐藏id框
    self.frmElement = options.frmElement;//需要验证的表单


    self.renderDiv = options.renderDiv;

    //removeData：删除操作完成后将数据从recordSet中移除
    //replaceData：修改操作后更新recordSet中相应记录
    self.removeData = options.removeData;
    self.replaceData = options.replaceData;

    self.highLighNodeId = 0;//高亮结点id

    if (typeof (options.showDetailEnd) != "undefined" && options.showDetailEnd != "") {

        self.showDetailEnd = options.showDetailEnd;
    }
   //显示对话框详细之后


    //初始化
    self.init = function () {

        $.ajax(
       {
           url: self.dataQueryUrlAccessor(self)
           , cache: false
           , type: "GET"
           , success: function (data, text) {
               self.zTreeDiv = $.fn.zTree.init(self.renderDiv, ztSetting, data);


               var selNode = self.zTreeDiv.getNodeByParam("id", self.highLighNodeId, null); //如果有高度节点，则选中它
               self.zTreeDiv.selectNode(selNode);

               if (self.highLighNodeId != 0 || self.highLighNodeId != "0") {
                   self.showDetail(self.highLighNodeId);
               }

           }
           , error: function () {
               alert("获取相关树数据出错！");
           }
       });

    };

   //显示结点详细信息
   self.showDetail = function (selectNodeId) {
       //如果不是根,则需要取数据
       $.ajax(
       {
           url: self.dataNodeSelect(selectNodeId)
           , type: "GET"
           , cache: false
           , success: function (data) {
               self.NodeValue(data);
               if (self.showDetailEnd) {
                   self.showDetailEnd(data);
               }
             
           }
           , error: function () {
               alert("获取相关节点数据出错！可能是服务器无反应!!");
           }
       });
   }
  
    //选中结点事件
   self.zTreeOnClick = function (event, treeId, treeNode) {

       self.hidId.val(treeNode.id); //保存选择id

       if (treeNode.id != 0) {

           self.showDetail(treeNode.id);

       }
   };




    //保存更新事件
    self.updateNode = function () {
        //(1)读取更新

        if ( self.frmElement.valid() ) {

            self.UpdateBtn.attr('disabled', 'disabled');
            self.DelBtn.attr('disabled', 'disabled');
            self.AddBtn.attr('disabled', 'disabled');

            var jsonData = JSON.stringify(self.NodeValue());
            if (self.hidId.val() < 1) {
                alert('请选择一个非根结点后，再保存！');

                self.UpdateBtn.removeAttr("disabled");
                self.DelBtn.removeAttr("disabled");
                self.AddBtn.removeAttr("disabled");

                return false;
            }
            $.ajax(
           {
               url: self.dataUpdateAccessor(self.hidId.val())
               , type: "POST"
               , contentType: "application/json; charset=utf-8"

               , data: jsonData
               , success: function (data, text) {

                   //$.smallBox({
                   //    title: "提示",
                   //    content: "<i class='fa fa-clock-o'></i> <i>修改信息成功！</i>",
                   //    color: "#659265",
                   //    iconSmall: "fa fa-check fa-2x fadeInRight animated",
                   //    timeout: 3000
                   //});

                   $.bigBox({
                       title: "提示",
                       content: "<i class='fa fa-check'></i> <i>修改信息成功！</i>",
                       color: "#739E73",
                       
                       icon: "fa fa-check",
                       
                       timeout: 2000
                   });

                   
                   self.highLighNodeId = self.hidId.val();
                   self.zTreeDiv.destroy();
                   self.init();

               }
               

           } ).always( function () {
               self.UpdateBtn.removeAttr( "disabled" );
               self.DelBtn.removeAttr( "disabled" );
               self.AddBtn.removeAttr( "disabled" );

            } );

        }


    };



   //保存新建事件.AddNewNode
    self.AddNewNode = function () {

        if ($.trim(self.hidId.val()) == "") {
            alert("出错了，请先选中一个节点，再为它加上子节点！！");
            return;
        };

        self.UpdateBtn.attr( 'disabled', 'disabled' );
        self.DelBtn.attr( 'disabled', 'disabled' );
        self.AddBtn.attr( 'disabled', 'disabled' );


        $.ajax(
       {
           url: self.dataAddUrlAccessor(self.hidId.val())
           , type: "POST"
           , contentType: "application/json; charset=utf-8"

           , success: function (data, text) {

               $.bigBox({
                   title: "提示",
                   content: "<i class='fa fa-check'></i> <i>新增结点成功！</i>",
                   color: "#739E73",

                   icon: "fa fa-check",

                   timeout: 2000
               });
               
               self.highLighNodeId = data;
               self.hidId.val(data);

               self.zTreeDiv.destroy();
               self.init();

            

           }

       } ).always( function () {
           self.UpdateBtn.removeAttr( "disabled" );
           self.DelBtn.removeAttr( "disabled" );
           self.AddBtn.removeAttr( "disabled" );

       } );


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
           , type: "POST"

           , success: function (data, text) {

               if(data>=0){
                   $.bigBox({
                       title: "提示",
                       content: "<i class='fa fa-check'></i> <i>删除结点成功！</i>",
                       color: "#739E73",

                       icon: "fa fa-check",

                       timeout: 2000
                   });


                   self.highLighNodeId = data;
                   self.hidId.val(data);
                   self.zTreeDiv.destroy();
                   self.init();
               }else{
                   switch (data) {
                       case "-1":
                           alert("无法删除空节点！");
                           break;
                       case "-3":
                           alert("无法删除有子节点的树，请Delete All Children Nodes！");
                           break;

                       default:
                           alert("未知错误！");
                           break;
        
                   }
                 
               }



           }
           , error: function (xhr, textStatus, errorThrown) {

               if (xhr.status == 409) {
                   alert("为安全，无法直接删除有子结点的树！！");
               } else {
                   alert("出错了！");
               }

           }

       } ).always( function () {
           self.UpdateBtn.removeAttr( "disabled" );
           self.DelBtn.removeAttr( "disabled" );
           self.AddBtn.removeAttr( "disabled" );

       } );

    }

}
