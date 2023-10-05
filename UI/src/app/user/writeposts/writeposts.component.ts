import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { Router } from '@angular/router';
import { LocalStorageService } from 'angular-web-storage';

@Component({
  selector: 'app-writeposts',
  templateUrl: './writeposts.component.html',
  styleUrls: ['./writeposts.component.css']
})
export class WritepostsComponent implements OnInit {

  fileSelected: boolean = false;


  


  selectedFileName: string | undefined;
  selectedImage: any;

  public postStoryForm!: FormGroup;

  public serverErrors: string[] = [];
  public showSuccessMsg = false;
  public showFailureMsg = false;
  public successMsg: string = "";

  constructor(private fb: FormBuilder, private api: ApiService,
    private router: Router, private localStorage: LocalStorageService
  ) { }
  ngOnInit(): void {
    

    this.postStoryForm = this.fb.group({
      pTitle: ["", Validators.required],
      pDescription: ["", Validators.required]
      
    });

  }
  get hcForm() {
    return this.postStoryForm.controls;
  }
  formData = new FormData();
  upload(file: File): void  {

    
    this.fileSelected = true;


    this.formData.append("PP", file);
    
  }


  postStory() {
   
    this.formData.append('myModel', JSON.stringify(this.postStoryForm.value));
    this.api.post(this.formData).subscribe(
      res => {
      },
      err => {
        this.formData.delete("PP");
        this.formData.delete("myModel");
        this.serverErrors = [];
        if (err.status === 400) {
          Object.keys(err.error.errors).forEach(key => {
            this.serverErrors.push(err.error.errors[key][0]);
          });
        }
        else if (err.status === 500) {
          console.log(err);
          this.serverErrors.push(err.error);
        }
        else if (err.status === 0) {
          console.log(err);
          this.serverErrors.push("API Service seems to be down.");
        }
        else {
          this.serverErrors.push(err.message);
        }
        this.showFailureMsg = true;
        this.showSuccessMsg = false;
      },
      () => {
        debugger;
        this.formData.delete("PP");
        this.formData.delete("myModel");
        this.postStoryForm.reset();
        this.successMsg = "Story Posted Successfully!";
        this.showFailureMsg = false;
        this.showSuccessMsg = true;
        setTimeout(() => {
          this.showSuccessMsg = false;
        }, 5000);
        
        this.selectedFileName = 'Choose an image';
        this.selectedImage = null;
        this.router.navigate(['/Explore']);

      }
    );
  }



  previewImage(fileInput: HTMLInputElement): void {
    const file = fileInput.files ? fileInput.files[0] : null;
    if (file) {
      this.selectedFileName = file.name;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.selectedImage = e.target.result;
      };

      reader.readAsDataURL(file);

      // Call the upload function separately after the image preview
      this.upload(file);
    }

    else {
      this.selectedFileName = 'Choose an image';
      this.selectedImage = null;
    }
  }
}
