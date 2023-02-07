//-Christian Leo
class DropDownModel {
    constructor(btn, div, nameNormal) {
        this.btn = btn;
        this.div = div;
        this.nameNormal = nameNormal;
    }

    current = 0;


    auto() {
        if (this.current === 0) {
            this.show();
        }
        else {
            this.hide();
        }
    }




    //- Showing the DropDown Element
    show() {
        this.btn.innerHTML = 'Luk &#8595';
        this.btn.style.color = 'orange';
        this.div.style.display = 'inline';
        this.current = 1;
    }

    //- Hiding the DropDown Element
    hide() {
        this.btn.innerHTML = this.nameNormal;
        this.btn.style.borderBottom = 'none';
        this.btn.style.color = 'white';
        this.div.style.display = 'none';
        this.current = 0;
    }
}

var elements = [
    new DropDownModel
    (
        document.getElementById('btndropdown'),
        document.getElementById('adminMenu'),
        'Admin'
    ),

    new DropDownModel
    (
        document.getElementById('btnInfoMenu'),
        document.getElementById('infoMenu'),
        'Info-Sk\u00e6rm(live)'
    ),

    new DropDownModel
    (
        document.getElementById('btnActivity'),
        document.getElementById('activityMenu'),
        'Aktiviteter'
    )
]
//Using this to check if any are shown

function actionHandler(target) {
    for (var i = 0; i < elements.length; i++) {
        if (i != target) {
            if (elements[i].current === 1)
                elements[i].hide();
        }
    }
    elements[target].auto();
}









