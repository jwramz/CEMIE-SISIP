/*
 * Application Level Javascript Modules
 */


/* Configure jQuery-Datepicker for spanish */
$(function () {
    try {
        $.datepicker.regional['es'] = {
            closeText: 'Cerrar',
            prevText: '< Ant',
            nextText: 'Sig >',
            currentText: 'Hoy',
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
            weekHeader: 'Sm',
            dateFormat: 'yy-mm-dd',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional["es"]);
    } catch (err) { /*DO-NOTHING */}
});

function showContainerPrueba(containerSelector, liSelector) {
    $(".container-prueba").hide();
    $(".menu-left-pruebas li").removeClass("active");

    if ($(containerSelector)[0]) {            
        $(containerSelector).show();
        $(liSelector).closest("li").addClass("active");
    }
}

function showInspeccionVisual(containerSelector) {
    $(".container-prueba").hide();
    if ($(containerSelector)[0]) {
        $(containerSelector).show();        
    }
}


