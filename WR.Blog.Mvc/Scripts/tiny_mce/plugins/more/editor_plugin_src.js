/**
 * editor_plugin_src.js
 *
 * Copyright 2009, Moxiecode Systems AB
 * Released under LGPL License.
 *
 * License: http://tinymce.moxiecode.com/license
 * Contributing: http://tinymce.moxiecode.com/contributing
 */

(function() {
    tinymce.create('tinymce.plugins.MorePlugin', {
        init : function(ed, url) {
            var pb = '<img src="' + ed.theme.url + '/img/trans.gif" class="mcePageBreak mceItemNoResize" />', cls = 'mceMore', sep = ed.getParam('pagebreak_separator', '<!--more-->'), pbRE;

            pbRE = new RegExp(sep.replace(/[\?\.\*\[\]\(\)\{\}\+\^\$\:]/g, function(a) {return '\\' + a;}), 'g');

            // Register commands
            ed.addCommand('mceMore', function() {
                ed.execCommand('mceInsertContent', 0, pb);
            });

            // Register buttons
            ed.addButton('more', { title: 'More tag to summarize post.', cmd: cls, image: url + '/more.gif' });

            ed.onInit.add(function() {
                if (ed.theme.onResolveName) {
                    ed.theme.onResolveName.add(function(th, o) {
                        if (o.node.nodeName == 'IMG' && ed.dom.hasClass(o.node, cls))
                            o.name = 'more';
                    });
                }
            });

            ed.onClick.add(function(ed, e) {
                e = e.target;

                if (e.nodeName === 'IMG' && ed.dom.hasClass(e, cls))
                    ed.selection.select(e);
            });

            ed.onNodeChange.add(function(ed, cm, n) {
                cm.setActive('more', n.nodeName === 'IMG' && ed.dom.hasClass(n, cls));
            });

            ed.onBeforeSetContent.add(function(ed, o) {
                o.content = o.content.replace(pbRE, pb);
            });

            ed.onPostProcess.add(function(ed, o) {
                if (o.get)
                    o.content = o.content.replace(/<img[^>]+>/g, function(im) {
                        if (im.indexOf('class="mcePageBreak') !== -1)
                            im = sep;

                        return im;
                    });
            });
        },

        getInfo : function() {
            return {
                longname : 'More',
                author : 'Moxiecode Systems AB',
                authorurl : 'http://tinymce.moxiecode.com',
                infourl : 'http://wiki.moxiecode.com/index.php/TinyMCE:Plugins/pagebreak',
                version : tinymce.majorVersion + "." + tinymce.minorVersion
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('more', tinymce.plugins.MorePlugin);
})();