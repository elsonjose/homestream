import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import {
  DIALOG_WIDTH,
  MenuAction,
  TABLE_CONTENT_TEXT_LENGTH,
  TABLE_HEADER_TEXT_LENGTH,
} from 'src/app/common/constants';
import { Directory } from 'src/app/models/directory';
import { Menu } from 'src/app/models/menu';
import { AddFolderDialogComponent } from '../add-folder-dialog/add-folder-dialog.component';

@Component({
  selector: 'home-stream-add-folder',
  templateUrl: './add-folder.component.html',
  styleUrls: ['./add-folder.component.scss'],
})
export class AddFolderComponent implements OnInit {
  menu: Menu[] = [
    {
      Text: 'Add Folder',
      Action: MenuAction.ADD_FOLDER,
    },
    {
      Text: 'Search All',
      Action: MenuAction.SEARCH_ALL,
    },
  ];
  TABLE_HEADER_LENGTH = TABLE_HEADER_TEXT_LENGTH;
  TABLE_CONTENT_LENGTH = TABLE_CONTENT_TEXT_LENGTH;
  tableHeaders = ['Folder paths selected for searching', 'Actions'];
  directories: Directory[] = [
    {
      id: '123',
      path: 'C://123/',
    },
    {
      id: '123',
      path: 'C://123/',
    },
    {
      id: '123',
      path: 'C://123/',
    },
    {
      id: '123',
      path: 'C://123/',
    },
    {
      id: '123',
      path: 'C://123/',
    },
  ];

  constructor(private dialog: MatDialog) {}

  ngOnInit(): void {}

  onMenuAction(actionId: number) {
    switch (actionId) {
      case MenuAction.ADD_FOLDER: {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.width = DIALOG_WIDTH;
        var dialogRef = this.dialog.open(
          AddFolderDialogComponent,
          dialogConfig
        );
        dialogRef.afterClosed().subscribe({
          next: (dir: Directory) => {
            this.directories.push(dir);
          },
          error: (e) => console.error(e),
          complete: () => console.info('complete'),
        });
        return;
      }
    }
  }
}
