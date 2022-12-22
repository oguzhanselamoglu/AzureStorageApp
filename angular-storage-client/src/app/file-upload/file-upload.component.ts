import { HttpEventType, HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { FileUploadService } from '../services/file-upload.service';
import { FileSelectComponent } from './file-select/file-select.component';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit {
  selectedFiles?: FileList;
  currentFile?: File;
  progress = 0;
  message = '';

  fileInfos?: Observable<any>;

  constructor(private uploadService: FileUploadService,public dialog: MatDialog) { }

  ngOnInit() {
    this.refresh();
  }
  refresh(): void {
    this.fileInfos = this.uploadService.getFiles();
  }
  selectFile(event: any): void {
    this.selectedFiles = event.target.files;
  }
  addData(): void {


    const dialogRef = this.dialog.open(FileSelectComponent, {
      width: '250px',

    });

    dialogRef.afterClosed().subscribe((result: { status: any; }) => {
      console.log('The dialog was closed');
      if (result.status) {
        this.upload();
        //TODO: modal üzerinden zaten veri geliyor fakat ngrx üzerinden almaya çalıştım
        // this.dataSource.push(result.data);
        // this.table.renderRows();

      }

    });
  }

  upload(): void {
    this.progress = 0;

    if (this.selectedFiles) {
      const file: File | null = this.selectedFiles.item(0);

      if (file) {
        this.currentFile = file;

        this.uploadService.upload(this.currentFile).subscribe({
          next: (event: any) => {
            if (event.type === HttpEventType.UploadProgress) {
              this.progress = Math.round(100 * event.loaded / event.total);
            } else if (event instanceof HttpResponse) {
              this.message = event.body.message;
              this.fileInfos = this.uploadService.getFiles();
            }
          },
          error: (err: any) => {
            console.log(err);
            this.progress = 0;

            if (err.error && err.error.message) {
              this.message = err.error.message;
            } else {
              this.message = 'Could not upload the file!';
            }

            this.currentFile = undefined;
          }
        });
      }

      this.selectedFiles = undefined;
    }
  }
  download(name: string): void {
    this.uploadService.download(name).subscribe(response => this.downLoadFile(response, name))
  }
  downLoadFile(data: any, name: string) {
    // let blob = new Blob([data]);
    // let url = window.URL.createObjectURL(blob);
    // let pwa = window.open(url);
    // if (!pwa || pwa.closed || typeof pwa.closed == 'undefined') {
    //     alert( 'Please disable your Pop-up blocker and try again.');
    // }

    let binaryData = [];
    binaryData.push(data);
    let downloadLink = document.createElement('a');
    downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, { type: 'blob' }));
    downloadLink.setAttribute('download', name);
    document.body.appendChild(downloadLink);
    downloadLink.click();
  }
  delete(name: string): void {
    this.uploadService.delete(name).subscribe(response => this.refresh());
  }
}
