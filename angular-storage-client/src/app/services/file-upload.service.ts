import { HttpClient, HttpEvent, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  private baseUrl = 'http://localhost:5157';
  constructor(private http: HttpClient) { }

  upload(file: File): Observable<HttpEvent<any>> {
    const formData: FormData = new FormData();

    formData.append('file', file);

    const req = new HttpRequest('POST', `${this.baseUrl}/api/Blobs/Upload`, formData, {
      reportProgress: true,
      responseType: 'json'
    });

    return this.http.request(req);
  }
  getFiles(): Observable<any> {
    return this.http.get(`${this.baseUrl}/api/Blobs/GetNames`);
  }
  download(name: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/api/Blobs/Download?fileName=` + name,{responseType: 'blob' as 'json'});
  }
  delete(name: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/api/Blobs/Delete?fileName=` + name);
  }
}
