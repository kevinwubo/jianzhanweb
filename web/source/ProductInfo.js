

var productinfo = {
    init: function () {
        productinfo.regEvent();
    },

    regEvent: function () {

        //业界大师
        $("#div_yjds").on("click", function () {
            var infos = JSON.parse($("#hid_yjds").val());
            if (infos != "") {
                $("#div_showinfo").html("");
                for (var i = 0; i < infos.length; i++) {
                    $("#div_showinfo").append("<div class='markitem'><a href='/WebHome/mn_shop?author=" + infos[i].artisanName + "'>" + infos[i].artisanName + "</div>");
                }
            }
        });

        //老牌传承人
        $("#div_lpccr").on("click", function () {
            var infos = JSON.parse($("#hid_lpccr").val());
            if (infos != "") {
                $("#div_showinfo").html("");
                for (var i = 0; i < infos.length; i++) {
                    $("#div_showinfo").append("<div class='markitem'><a href='/WebHome/mn_shop?author=" + infos[i].artisanName + "'>" + infos[i].artisanName + "</div>");
                }
            }
        });
        //名家工艺师
        $("#div_mjgys").on("click", function () {
            var infos = JSON.parse($("#hid_mjgys").val());
            if (infos != "") {
                $("#div_showinfo").html("");
                for (var i = 0; i < infos.length; i++) {
                    $("#div_showinfo").append("<div class='markitem'><a href='/WebHome/mn_shop?author=" + infos[i].artisanName + "'>" + infos[i].artisanName + "</div>");
                }
            }
        });
        //器形/釉色
        $("#div_qxys").on("click", function () {
            var infoqxs = JSON.parse($("#hid_qx").val());
            var infoyss = JSON.parse($("#hid_ys").val());
            if (infoqxs != "") {
                $("#div_showinfo").html("");
                for (var i = 0; i < infoqxs.length; i++) {
                    $("#div_showinfo").append("<div class='markitem'><a href='/WebHome/mn_shop?type3=" + infoqxs[i].ValueInfo + "'>" + infoqxs[i].ValueInfo + "</div>");
                }
            }
            if (infoyss != "") {
                for (var i = 0; i < infoyss.length; i++) {
                    $("#div_showinfo").append("<div class='markitem'><a href='/WebHome/mn_shop?type2=" + infoyss[i].ValueInfo + "'>" + infoyss[i].ValueInfo + "</div>");
                }
            }
        });

    }
}
