import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Menu } from 'src/app/models/menu';
import { appName } from 'src/main';

@Component({
  selector: 'home-stream-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent implements OnInit {
  @Input() menu: Menu[] = [];
  @Output() menuEventEmitter = new EventEmitter<number>();

  appName = appName;

  constructor() {}

  ngOnInit(): void {
    console.log(this.menu.length);
  }

  onMenuItemClicked(action: number) {
    this.menuEventEmitter.emit(action);
  }
}
