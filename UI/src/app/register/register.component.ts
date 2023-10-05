import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  public userRegistrationForm!: FormGroup;

  fileSelected: boolean = false;


  selectedFileName: string | undefined;
  selectedImage: any;

  constructor(private fb: FormBuilder, private authService: AuthService,    
    private router: Router
  ) { }

  ngOnInit(): void {
    this.userRegistrationForm = this.fb.group({
      userName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required]]
      
    });
  }

  get urf() {
    return this.userRegistrationForm.controls;
  }

  formData = new FormData();

  upload(file: File): void {
    this.fileSelected = true;

    // Append the file to the FormData
    this.formData.append('PP', file);

    // Display FormData in the console
    console.log(this.formData);
  }

  register() {

    this.formData.append('myModel', JSON.stringify(this.userRegistrationForm.value));
    
    this.authService.register(this.formData).subscribe(
      res => {

      },
      err => {
        this.formData.delete("PP");
        this.formData.delete("myModel");
      },
      () => {
        alert('User Created Successfully');
        this.router.navigate(['./login']);
      });
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
    } else {
      this.selectedFileName = 'Choose an image';
      this.selectedImage = null;
    }
  }
}
