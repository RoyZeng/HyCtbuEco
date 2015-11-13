﻿function updateCoords(n) {
    var tempId = $("#ReturnId").val();
    jQuery("#x").val(n.x), jQuery("#y").val(n.y), jQuery("#w").val(n.w), jQuery("#h").val(n.h), jQuery("#IBrandID").val(tempId)
}
var croper;
function CropImage() {
    new Uploader(
        $("#jquery-wrapped-fine-uploader"),
        function (n) {
            $("#preview_title").html("头像预览"),
            $("img.preview-image").each(function () {
                this.src = n
            });
            $("#imgsrc").val(n), croper ? croper.setImage(n) : croper = new Croper($("#crop_image"), new Previewer([[$("#preview_large"), 180]]));
            $("#crop_operation_submit").bind("click", function () {
                $("#crop_operation_msg").html("操作中...").show()
            }),
            $("#crop_operation").show(),
            $("#croped_message").html("")
        });

    $("#form_crop").ajaxForm({
        success: function (n) {
            $("#crop_image").attr("src", "");
            croper.setImage("");
            $("#preview_large").removeAttr("style").attr("src", n.AvatarSrc);
            $("#crop_operation").hide();
            $("#crop_operation_msg").html("");
            $("#croped_message").html("裁切并保存成功");
            $("#preview_title").html("更新后的Logo");
         
              window.location.reload();
            

        }
    })
}
    var Uploader = function () {
        var n = function (n, t) {
            n.fineUploader({
                validation: {
                    allowedExtensions: ["png", "gif", "jpg", "jpeg"],
                    sizeLimit: 10485760
                },
                request: {
                    endpoint: "/api/BrandAndGoodsExt/ProcessUpload"
                },
                text: {
                    uploadButton: '<i class="icon-upload icon-white"><\/i> 点击上传图片',
                    dropProcessing: "（支持文件拖放上传，只能上传单张10M以下png、jpg、gif图片）"
                },
                template: '<div class="qq-uploader span12"><pre class="qq-upload-drop-area span12"><span>{dragZoneText}<\/span><\/pre><div class="qq-upload-button btn btn-success" style="width: 100px;">{uploadButtonText}<\/div><span class="qq-drop-processing"><span>{dropProcessingText}<\/span><span class="qq-drop-processing-spinner"><\/span><\/span><ul class="qq-upload-list" style="margin-top: 10px;overflow:hidden;"><\/ul><\/div>',
                classes: {
                    success: "alert alert-success",
                    fail: "alert alert-error"
                },
                multiple: !1
            }).on("complete", function (n, i, r, u) {
                if (u.success) {
                    var f = u.message;
                    f += "?id=" + (new Date).getTime() + Math.floor(Math.random() * 1e3);
                    t(f);
                }
                else
                    $("#message").html(u.message)
            })
        };
        return n.prototype = {
            constructor: n
        }, n
    }(),
        Previewer = function () {
            var n, t = function (t) {
                n = t
            };
            return t.prototype = {
                constructor: t,
                showAllPreview: function (t) {
                    var i = this.getWidgetSize(),
                        r;
                    width = i[0], height = i[1], r = function (n, t, i) {
                        if (parseInt(n.w) > 0) {
                            var r = i / n.w,
                                u = i / n.h;
                            t.css({
                                width: Math.round(r * width) + "px",
                                height: Math.round(u * height) + "px",
                                marginLeft: "-" + Math.round(r * n.x) + "px",
                                marginTop: "-" + Math.round(u * n.y) + "px"
                            }).show()
                        }
                    }, $.each(n, function () {
                        r(t, this[0], this[1])
                    })
                },
                hideAllPreview: function () {
                    $.each(n, function () {
                        this[0].stop().fadeOut("fast")
                    })
                }
            }, t
        }(),
        Croper = function () {
            var n, t, i = function (i, r) {
                t = this, i.Jcrop({
                    onChange: r.showAllPreview,
                    onSelect: updateCoords,
                    aspectRatio: 1
                }, function () {
                    n = this, t.setSelect()
                })
            };
            return i.prototype = {
                constructor: i,
                setImage: function (i) {
                    n.setImage(i, function () {
                        t.setSelect()
                    })
                },
                setSelect: function () {
                    var t = Math.min.apply(Math, n.getWidgetSize());
                    n.setSelect([0, 0, t, t])
                }
            }, i
        }();
