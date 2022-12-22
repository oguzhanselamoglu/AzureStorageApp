import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FileSelectDto } from '../models/FileSelectDto';

@Component({
  selector: 'app-file-select',
  templateUrl: './file-select.component.html',
  styleUrls: ['./file-select.component.css']
})
export class FileSelectComponent implements OnInit {

  data: FileSelectDto = new FileSelectDto();
  selectedFiles?: FileList;
  constructor( public dialogRef: MatDialogRef<FileSelectComponent>,) { }

  ngOnInit() {
  }
  selectFile(event: any): void {
    this.selectedFiles = event.target.files;
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
upload(): void {

}

}
