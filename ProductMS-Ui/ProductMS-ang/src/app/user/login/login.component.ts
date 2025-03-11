import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  encapsulation: ViewEncapsulation.Emulated,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  form: FormGroup; 
  isSubmitted: boolean = false;

  constructor(
    private formBuilder: FormBuilder, 
    private authService: AuthService,  
    private router: Router,
    private toastr: ToastrService,
  ) { 
    this.form = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]], 
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigateByUrl('/dashboard');
    }
  }

  hasDisplayableError(controlName: string): boolean {
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched));
  }

  onSubmit() { 
    this.isSubmitted = true;
    if (this.form.invalid) {
      if (this.form.get('email')?.invalid) {
        this.toastr.error('Please enter a valid email address.', 'Validation Failed');
      }
      if (this.form.get('password')?.invalid) {
        this.toastr.error('Password is required.', 'Validation Failed');
      }
      return;
    }

    this.authService.login(this.form.value).subscribe({
      next: (response) => {
        console.log('Login successful', response);
        this.authService.setToken(response.token);  // Store the token
        this.router.navigate(['/products']);  // Redirect to dashboard
        this.toastr.success('Login successful!', 'Success');
      },
      error: (err) => {
        if (err.status === 400) {
          alert('An unexpected error occurred. Please try again later.');
        } else {
          this.toastr.error('Incorrect email or password.');
        }
      }
    });
  }
}
