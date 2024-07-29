/*
 *   This content is licensed according to the W3C Software License at
 *   https://www.w3.org/Consortium/Legal/2015/copyright-software-and-document
 *
 *   File:   Tree.js
 *
 *   Desc:   Tree widget that implements ARIA Authoring Practices
 *           for a tree being used as a file viewer
 */

/* global Treeitem */

'use strict';

/**
 * ARIA Treeview example
 *
 * @function onload
 * @description  after page has loaded initialize all treeitems based on the role=treeitem
 */

window.addEventListener('load', function () {
    var trees = document.querySelectorAll('[role="tree"]');

    for (var i = 0; i < trees.length; i++) {
        var t = new Tree(trees[i]);
        t.init();
    }
});

/*
 *   @constructor
 *
 *   @desc
 *       Tree item object for representing the state and user interactions for a
 *       tree widget
 *
 *   @param node
 *       An element with the role=tree attribute
 */

var Tree = function (node) {
    // Check whether node is a DOM element
    if (typeof node !== 'object') {
        return;
    }

    this.domNode = node;

    this.treeitems = [];
    this.firstChars = [];

    this.firstTreeitem = null;
    this.lastTreeitem = null;
    this.selectedItem = null;
};

Tree.prototype.init = function () {
    function findTreeitems(node, tree, group) {
        var elem = node.firstElementChild;
        var ti = group;

        while (elem) {
            if (elem.tagName.toLowerCase() === 'li') {
                ti = new Treeitem(elem, tree, group);
                ti.init();
                tree.treeitems.push(ti);
                tree.firstChars.push(ti.label.substring(0, 1).toLowerCase());
            }

            if (elem.firstElementChild) {
                findTreeitems(elem, tree, ti);
            }

            elem = elem.nextElementSibling;
        }
    }

    // initialize pop up menus
    if (!this.domNode.getAttribute('role')) {
        this.domNode.setAttribute('role', 'tree');
    }

    findTreeitems(this.domNode, this, false);

    this.updateVisibleTreeitems();

    this.firstTreeitem.domNode.tabIndex = 0;
};

Tree.prototype.setSelectedToItem = function (treeitem) {
    if (this.selectedItem) {
        this.selectedItem.domNode.setAttribute('aria-selected', 'false');
    }
    treeitem.domNode.setAttribute('aria-selected', 'true');
    this.selectedItem = treeitem;
};

Tree.prototype.setFocusToItem = function (treeitem) {
    for (var i = 0; i < this.treeitems.length; i++) {
        var ti = this.treeitems[i];

        if (ti === treeitem) {
            ti.domNode.tabIndex = 0;
            ti.domNode.focus();
        } else {
            ti.domNode.tabIndex = -1;
        }
    }
};

Tree.prototype.setFocusToNextItem = function (currentItem) {
    var nextItem = false;

    for (var i = this.treeitems.length - 1; i >= 0; i--) {
        var ti = this.treeitems[i];
        if (ti === currentItem) {
            break;
        }
        if (ti.isVisible) {
            nextItem = ti;
        }
    }

    if (nextItem) {
        this.setFocusToItem(nextItem);
    }
};

Tree.prototype.setFocusToPreviousItem = function (currentItem) {
    var prevItem = false;

    for (var i = 0; i < this.treeitems.length; i++) {
        var ti = this.treeitems[i];
        if (ti === currentItem) {
            break;
        }
        if (ti.isVisible) {
            prevItem = ti;
        }
    }

    if (prevItem) {
        this.setFocusToItem(prevItem);
    }
};

Tree.prototype.setFocusToParentItem = function (currentItem) {
    if (currentItem.groupTreeitem) {
        this.setFocusToItem(currentItem.groupTreeitem);
    }
};

Tree.prototype.setFocusToFirstItem = function () {
    this.setFocusToItem(this.firstTreeitem);
};

Tree.prototype.setFocusToLastItem = function () {
    this.setFocusToItem(this.lastTreeitem);
};

Tree.prototype.expandTreeitem = function (currentItem) {
    if (currentItem.isExpandable) {
        currentItem.domNode.setAttribute('aria-expanded', true);
        this.updateVisibleTreeitems();
    }
};

Tree.prototype.expandAllSiblingItems = function (currentItem) {
    for (var i = 0; i < this.treeitems.length; i++) {
        var ti = this.treeitems[i];

        if (ti.groupTreeitem === currentItem.groupTreeitem && ti.isExpandable) {
            this.expandTreeitem(ti);
        }
    }
};

Tree.prototype.collapseTreeitem = function (currentItem) {
    var groupTreeitem = false;

    if (currentItem.isExpanded()) {
        groupTreeitem = currentItem;
    } else {
        groupTreeitem = currentItem.groupTreeitem;
    }

    if (groupTreeitem) {
        groupTreeitem.domNode.setAttribute('aria-expanded', false);
        this.updateVisibleTreeitems();
        this.setFocusToItem(groupTreeitem);
    }
};

Tree.prototype.updateVisibleTreeitems = function () {
    this.firstTreeitem = this.treeitems[0];

    for (var i = 0; i < this.treeitems.length; i++) {
        var ti = this.treeitems[i];

        var parent = ti.domNode.parentNode;

        ti.isVisible = true;

        while (parent && parent !== this.domNode) {
            if (parent.getAttribute('aria-expanded') == 'false') {
                ti.isVisible = false;
            }
            parent = parent.parentNode;
        }

        if (ti.isVisible) {
            this.lastTreeitem = ti;
        }
    }
};

Tree.prototype.setFocusByFirstCharacter = function (currentItem, char) {
    var start, index;

    char = char.toLowerCase();

    // Get start index for search based on position of currentItem
    start = this.treeitems.indexOf(currentItem) + 1;
    if (start === this.treeitems.length) {
        start = 0;
    }

    // Check remaining slots in the menu
    index = this.getIndexFirstChars(start, char);

    // If not found in remaining slots, check from beginning
    if (index === -1) {
        index = this.getIndexFirstChars(0, char);
    }

    // If match was found...
    if (index > -1) {
        this.setFocusToItem(this.treeitems[index]);
    }
};

Tree.prototype.getIndexFirstChars = function (startIndex, char) {
    for (var i = startIndex; i < this.firstChars.length; i++) {
        if (this.treeitems[i].isVisible) {
            if (char === this.firstChars[i]) {
                return i;
            }
        }
    }
    return -1;
};
/*
 *   This content is licensed according to the W3C Software License at
 *   https://www.w3.org/Consortium/Legal/2015/copyright-software-and-document
 *
 *   File:   Treeitem.js
 *
 *   Desc:   Treeitem widget that implements ARIA Authoring Practices
 *           for a tree being used as a file viewer
 */

'use strict';

/*
 *   @constructor
 *
 *   @desc
 *       Treeitem object for representing the state and user interactions for a
 *       treeItem widget
 *
 *   @param node
 *       An element with the role=tree attribute
 */

var Treeitem = function (node, treeObj, group) {
    // Check whether node is a DOM element
    if (typeof node !== 'object') {
        return;
    }

    node.tabIndex = -1;
    this.tree = treeObj;
    this.groupTreeitem = group;
    this.domNode = node;
    this.label = node.textContent.trim();

    if (node.getAttribute('aria-label')) {
        this.label = node.getAttribute('aria-label').trim();
    }

    this.isExpandable = false;
    this.isVisible = false;
    this.inGroup = false;

    if (group) {
        this.inGroup = true;
    }

    var elem = node.firstElementChild;

    while (elem) {
        if (elem.tagName.toLowerCase() == 'ul') {
            elem.setAttribute('role', 'group');
            this.isExpandable = true;
            break;
        }

        elem = elem.nextElementSibling;
    }

    this.keyCode = Object.freeze({
        RETURN: 13,
        SPACE: 32,
        PAGEUP: 33,
        PAGEDOWN: 34,
        END: 35,
        HOME: 36,
        LEFT: 37,
        UP: 38,
        RIGHT: 39,
        DOWN: 40,
    });
};

Treeitem.prototype.init = function () {
    this.domNode.tabIndex = -1;

    if (!this.domNode.getAttribute('role')) {
        this.domNode.setAttribute('role', 'treeitem');
    }

    this.domNode.addEventListener('keydown', this.handleKeydown.bind(this));
    this.domNode.addEventListener('click', this.handleClick.bind(this));
    this.domNode.addEventListener('focus', this.handleFocus.bind(this));
    this.domNode.addEventListener('blur', this.handleBlur.bind(this));

    if (!this.isExpandable) {
        this.domNode.addEventListener('mouseover', this.handleMouseOver.bind(this));
        this.domNode.addEventListener('mouseout', this.handleMouseOut.bind(this));
    }
};

Treeitem.prototype.isExpanded = function () {
    if (this.isExpandable) {
        return this.domNode.getAttribute('aria-expanded') === 'true';
    }

    return false;
};

/* EVENT HANDLERS */

Treeitem.prototype.handleKeydown = function (event) {
    var flag = false,
        char = event.key;

    function isPrintableCharacter(str) {
        return str.length === 1 && str.match(/\S/);
    }

    function printableCharacter(item) {
        if (char == '*') {
            item.tree.expandAllSiblingItems(item);
            flag = true;
        } else {
            if (isPrintableCharacter(char)) {
                item.tree.setFocusByFirstCharacter(item, char);
                flag = true;
            }
        }
    }

    if (event.altKey || event.ctrlKey || event.metaKey) {
        return;
    }

    if (event.shift) {
        if (isPrintableCharacter(char)) {
            printableCharacter(this);
        }
    } else {
        switch (event.keyCode) {
            case this.keyCode.RETURN:
            case this.keyCode.SPACE:
                var treeitem = event.currentTarget;
                var label = treeitem.getAttribute('aria-label');
                if (!label) {
                    var child = treeitem.firstElementChild;
                    label = child ? child.innerText : treeitem.innerText;
                }
                document.getElementById('principal').innerText = label.trim();

                if (!this.isExpandable) this.tree.setFocusToItem(this);
                this.tree.setSelectedToItem(this);
                flag = true;
                break;

            case this.keyCode.UP:
                this.tree.setFocusToPreviousItem(this);
                flag = true;
                break;

            case this.keyCode.DOWN:
                this.tree.setFocusToNextItem(this);
                flag = true;
                break;

            case this.keyCode.RIGHT:
                if (this.isExpandable) {
                    if (this.isExpanded()) {
                        this.tree.setFocusToNextItem(this);
                    } else {
                        this.tree.expandTreeitem(this);
                    }
                }
                flag = true;
                break;

            case this.keyCode.LEFT:
                if (this.isExpandable && this.isExpanded()) {
                    this.tree.collapseTreeitem(this);
                    flag = true;
                } else {
                    if (this.inGroup) {
                        this.tree.setFocusToParentItem(this);
                        flag = true;
                    }
                }
                break;

            case this.keyCode.HOME:
                this.tree.setFocusToFirstItem();
                flag = true;
                break;

            case this.keyCode.END:
                this.tree.setFocusToLastItem();
                flag = true;
                break;

            default:
                if (isPrintableCharacter(char)) {
                    printableCharacter(this);
                }
                break;
        }
    }

    if (flag) {
        event.stopPropagation();
        event.preventDefault();
    }
};

Treeitem.prototype.handleClick = function (event) {
    if (this.isExpandable) {
        if (this.isExpanded()) {
            this.tree.collapseTreeitem(this);
        } else {
            this.tree.expandTreeitem(this);
        }
        event.stopPropagation();
    } else {
        this.tree.setFocusToItem(this);
    }
    this.tree.setSelectedToItem(this);
};

Treeitem.prototype.handleFocus = function () {
    var node = this.domNode;
    if (this.isExpandable) {
        node = node.firstElementChild;
    }
    node.classList.add('focus');
};

Treeitem.prototype.handleBlur = function () {
    var node = this.domNode;
    if (this.isExpandable) {
        node = node.firstElementChild;
    }
    node.classList.remove('focus');
};

Treeitem.prototype.handleMouseOver = function (event) {
    event.currentTarget.classList.add('hover');
};

Treeitem.prototype.handleMouseOut = function (event) {
    event.currentTarget.classList.remove('hover');
};
/*
 *   This content is licensed according to the W3C Software License at
 *   https://www.w3.org/Consortium/Legal/2015/copyright-software-and-document
 *
 *   File:   Treeitem.js
 *
 *   Desc:   Setup click events for Tree widget examples
 */

'use strict';

/**
 * ARIA Treeview example
 *
 * @function onload
 * @description  after page has loaded initialize all treeitems based on the role=treeitem
 */

window.addEventListener('load', function () {
    var treeitems = document.querySelectorAll('[role="treeitem"]');

    for (var i = 0; i < treeitems.length; i++) {
        treeitems[i].addEventListener('click', function (event) {
            var treeitem = event.currentTarget;
            var label = treeitem.getAttribute('aria-label');
            if (!label) {
                var child = treeitem.firstElementChild;
                label = child ? child.innerText : treeitem.innerText;
            }

            if ($(this).attr("data-idcode")) {

                openCode($(this).attr("data-idcode"));
            }

            event.stopPropagation();
            event.preventDefault();
        });
    }
});
var fileOpened = 0;
function openCode(id, line) {

    if (fileOpened == id)
    {
        colorLines(line);
    }
    else { 
        fileOpened = id;
        //document.getElementById('principal').innerText = label.trim();
        // Faz uma requisição AJAX para o endpoint
        $.ajax({
            url: endpointCode + '?id=' + id, // Insira o endpoint desejado aqui
            method: "GET",
            dataType: "html", // O tipo de dados esperado na resposta (JSON neste exemplo)
            success: function (response) {            
                // Sucesso na requisição AJAX
                // Insere o conteúdo retornado no elemento com o ID "content"
                $("#principal").html(response);

                document.querySelectorAll('pre.code').forEach(el => {
                    // then highlight each
                    hljs.highlightElement(el);
                    hljs.lineNumbersBlock(el);
                });
                $('[data-toggle="tooltip"]').tooltip();
                                
                if (line) 
                    setTimeout(function () {
                        colorLines(line);
                        highlightSup();
                    }, 500);         
            },
            error: function (xhr, status, error) {
                // Erro na requisição AJAX
                console.error("Erro na requisição AJAX: ", status, error);
            }
        });
    }
}


function highlightSup() {
    objSusp.A1.forEach(function (line, e) {
        var item = $('div[data-line-number=' + line[0] + ']');
        $('div[data-line-number=' + line[0] + ']').parent().parent().addClass('suspA1');
        $('div[data-line-number=' + line[0] + ']').parent().parent().attr('title', line[1]);
    })
    objSusp.A2.forEach(function (line, e) {
        var item = $('div[data-line-number=' + [0] + ']');
        $('div[data-line-number=' + line[0] + ']').parent().parent().addClass('suspA2');
        $('div[data-line-number=' + line[0] + ']').parent().parent().attr('title', line[1]);
    })
    objSusp.A3.forEach(function (line, e) {
        var item = $('div[data-line-number=' + line[0] + ']');
        $('div[data-line-number=' + line[0] + ']').parent().parent().addClass('suspA3');
        $('div[data-line-number=' + line[0] + ']').parent().parent().attr('title', line[1]);
    })
    objSusp.A4.forEach(function (line, e) {
        var item = $('div[data-line-number=' + line[0] + ']');
        $('div[data-line-number=' + line[0] + ']').parent().parent().addClass('suspA4');
        $('div[data-line-number=' + line[0] + ']').parent().parent().attr('title', line[1]);
    })
    objSusp.A5.forEach(function (line, e) {
        var item = $('div[data-line-number=' + line[0] + ']');
        $('div[data-line-number=' + line[0] + ']').parent().parent().addClass('suspA5');
        $('div[data-line-number=' + line[0] + ']').parent().parent().attr('title', line[1]);
    })
}
function defineSup() {

}

function colorLines(line) {
    $('.selected').each(function (e, x) { $(x).removeClass('selected') });
    $('div[data-line-number=' + line + ']').parent().parent().addClass('selected')

    var container = $(".top-panel");
    var item = $('div[data-line-number=' + line + ']');

    container.scrollTop(0);
    // Calcular a posição do item em relação à div de contêiner
    var itemOffset = item.offset().top - container.offset().top - 50;

    // Ajustar o valor do scrollTop para rolar até o item
    container.scrollTop(itemOffset);
}

$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    let alertFadeElement = document.getElementById("alertFadeContainer");

    if (alertFadeElement) {
        // Exibe o alerta
        alertFadeElement.style.display = "block";

        // Oculta o alerta após 5 segundos
        setTimeout(function () {
            $('#alertFadeContainer').fadeOut();
        }, 5000);
    }

})