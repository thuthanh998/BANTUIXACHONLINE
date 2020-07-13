// cong trừ số lượng giỏ hàng
var SL = 1;
function Tru() {
    SL = document.getElementById("txtSL").value;
    if (isNaN(SL) == true) {
        SL = 1;
    }
    SL--;
    if (SL < 1) {
        SL = 1;
    }
    document.getElementById("txtSL").value = SL;
};
function Cong() {
    SL = document.getElementById("txtSL").value;
    if (isNaN(SL) == true) {
        SL = 0;
    }
    SL++;
    document.getElementById("txtSL").value = SL;
};