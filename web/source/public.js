﻿//手机号加密 1:M 2:N 3:Q 4:F 5:G 6:K 7:L 8:P 9E 0:D
function TelJM(str) {
    var reg1 = /1/g;
    var reg2 = /2/g;
    var reg3 = /3/g;
    var reg4 = /4/g;
    var reg5 = /5/g;
    var reg6 = /6/g;
    var reg7 = /7/g;
    var reg8 = /8/g;
    var reg9 = /9/g;
    var reg0 = /0/g;
    str = str.replace(reg1, 'M');
    str = str.replace(reg2, 'N');
    str = str.replace(reg3, 'Q');
    str = str.replace(reg4, 'F');
    str = str.replace(reg5, 'G');
    str = str.replace(reg6, 'K');
    str = str.replace(reg7, 'L');
    str = str.replace(reg8, 'P');
    str = str.replace(reg9, 'E');
    str = str.replace(reg0, 'D');
    return str;
}

// 获取长度为len的随机字符串
function getRandomString(len) {
    len = len || 32;
    var $chars = 'ABCDEFGHJKMNPQRSTWXYZ'; // 默认去掉了容易混淆的字符oOLl,9gq,Vv,Uu,I1
    var maxPos = $chars.length;
    var pwd = '';
    for (i = 0; i < len; i++) {
        pwd += $chars.charAt(Math.floor(Math.random() * maxPos));
    }
    return pwd;
}

