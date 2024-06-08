import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ContentChange, QuillModules } from 'ngx-quill';

// emojis
import 'quill-emoji/dist/quill-emoji.js';

import Quill from 'quill';

// register resize image
import BlotFormatter from 'quill-blot-formatter/dist/BlotFormatter';

Quill.register('modules/blotFormatter', BlotFormatter);

@Component({
  selector: 'app-quill-editor',
  templateUrl: './quill-editor.component.html',
  styleUrl: './quill-editor.component.scss'
})
export class QuillEditorComponent {
  @Input() height: number = 400;
  @Input() text: string = '';
  @Input() readOnly: boolean = false;

  @Input() modules: QuillModules = <QuillModules>{
    'emoji-shortname': true,
    'emoji-textarea': false,
    'emoji-toolbar': true,
    blotFormatter: {
      // empty object for default behaviour.
    },
    toolbar: {
      container: [
        [{ 'size': ['small', false, 'large', 'huge'] }],
        [{ 'font': [] }],

        [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

        ['bold', 'italic', 'underline', 'strike'],

        [{ 'color': [] }, { 'background': [] }],

        ['blockquote', 'code-block'],

        [{ 'list': 'ordered' }, { 'list': 'bullet' }],
        [{ 'script': 'sub' }, { 'script': 'super' }],
        [{ 'indent': '-1' }, { 'indent': '+1' }],
        [{ 'direction': 'rtl' }],

        [{ 'align': [] }],

        ['clean'],

        ['link', 'image', 'video'],
        ['emoji']
      ]
    }
  };

  @Output() onTextChange: EventEmitter<string> = new EventEmitter<string>();

  changedEditor(event: ContentChange) {
    this.onTextChange.emit(this.text)
  }
}
