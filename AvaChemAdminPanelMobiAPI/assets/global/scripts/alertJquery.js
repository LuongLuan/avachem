function fisrtFunction() {
    if (window.location.href.indexOf('message')) {
        $("#ContentPlaceHolder1_successLabel").delay(500).hide(5000);
        $("#ContentPlaceHolder1_warningLabel").delay(500).hide(5000);
    }
   
}
window.onload = fisrtFunction;