/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.extraPlugins = 'insertpre';
    config.insertpre_class = 'prettyprint';

    config.toolbar_Basic = [
            ['Format', '-', 'Undo', 'Redo', '-', 'Bold', 'Italic', 'Underline', 'Strike', '-', 'Blockquote', 'NumberedList', 'BulletedList', '-', 'Image', 'Link', 'InsertPre']
    ];
};
