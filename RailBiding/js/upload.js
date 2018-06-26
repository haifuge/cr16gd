/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
function ajaxUpload(opt) {
    /*
     参数说明:
     opt.id : 页面里file控件的ID;
     opt.frameName : iframe的name值;
     opt.url : 文件要提交到的地址;
     opt.format : 文件格式，以数组的形式传递，如['jpg','png','gif','bmp'];
     opt.callBack : 上传成功后回调;
     */
    var iName = opt.frameName; //太长了，变短点
    var iframe, form, file, fileParent, sign_input, uuid;
    //创建iframe和form表单
    iframe = $('<iframe name="' + iName + '" id="' + iName + '" width="0" height="0" />');
    form = $('<form method="post" style="display:none;" target="' + iName + '" action="' + opt.url + '"  name="form_' + iName + '" enctype="multipart/form-data" />');
    file = $('#' + opt.id); //通过id获取flie控件
    sign_input = $('<input type="hidden" name="guid" value="" />');
    fileParent = file.parent(); //存父级
    file.appendTo(form);
    //插入body
    $(document.body).append(iframe).append(form);
    form.append(sign_input);

    //console.log(Date.parse(new Date())+file.prop("files")[0].name+"ssssssssssss");
    //return false;
    //取得所选文件的扩展名
    var fileFormat = /\.[a-zA-Z]+$/.exec(file.val())[0].substring(1);
    if (opt.format.join('-').indexOf(fileFormat) != -1) {
        uuid = window.uuid();
        sign_input.val(uuid);
        form.submit();//格式通过验证后提交表单;
    } else {
        file.appendTo(fileParent); //将file控件放回到页面
        iframe.remove();
        form.remove();
        alert('文件格式错误，请重新选择！');
    }
    ;


    //文件提交完后
    iframe.load(function () {
        //var data = $(this).contents().find('body').html();
        $.ajax({
            type: "get",
            url: host + "file/get",
            dataType: "jsonp",
            data: {Q_guid_EQ_String: uuid},
            success: function (data) {
                console.log(data);
                file.appendTo(fileParent);
                iframe.remove();
                form.remove();
                opt.callBack(data);
            }
        });
    })
}


function uuid() {
    var s = [];
    var hexDigits = "0123456789abcdef";
    for (var i = 0; i < 36; i++) {
        s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
    }
    s[14] = "4";  // bits 12-15 of the time_hi_and_version field to 0010
    s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);  // bits 6-7 of the clock_seq_hi_and_reserved to 01
    s[8] = s[13] = s[18] = s[23] = "-";

    var uuid = s.join("");
    return uuid;
}

//文件上传
$(".fileInput").change(function(){
       ajaxUpload({
               id:"fileInput",
               frameName:"upload",
               url:host + "file/upload",
               format:['jpg','png','gif','bmp'],
               callBack:function(data){
                       var li_htm = "<li><img src='"+ data.uri +"' /><a data-id='"+ data.id +"' class='js-img-del'>删除</a></li>";
                       $("ul.imglist").append(li_htm);
                       $("#fileInput_con").val(data.id);
                       
                       //图片删除
                        $(".js-img-del").click(function(){
                            var id = $(this).data("id");
                            var li_htm = $(this).parents("li");
                            li_htm.remove();
                           $.ajax({
                               type:"post",
                               url:host + "file/remove",
                               dateType:"jsonp",
                               data:{id:id},
                               success:function(){
                               }
                           }) 
                        });
               }
       });
});

//文件上传
$(".allfileInput").change(function(){
       ajaxUpload({
               id:"allfileInput",
               frameName:"upload",
               url:host + "file/upload",
               format:['jpg','png','gif','bmp','pdf','docx','doc','xlsx','txt'],
               callBack:function(data){
                       
                       var li_htm = "<li><a class='file-path' href='"+ data.uri +"'>"+ $("#allfileInput").val() +"</a><a class='del-img' data-id='"+ data.id +"' class='js-img-del'>删除</a></li>";
                       $("ul.imglist").append(li_htm);
                       $("#fileInput_con").val(data.id);
                       
                       //图片删除
                        $(".js-img-del").click(function(){
                            var id = $(this).data("id");
                            var li_htm = $(this).parents("li");
                            li_htm.remove();
                           $.ajax({
                               type:"post",
                               url:host + "file/remove",
                               dateType:"jsonp",
                               data:{id:id},
                               success:function(){
                               }
                           }) 
                        });
               }
       });
});



