import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import  { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
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

  hasDisplayableError(controlName: string): boolean {
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched));
  }

  onSubmit() { 
    this.isSubmitted = true;
    if (this.form.invalid) return;

    this.authService.login(this.form.value).subscribe({  
      next: (response) => {
        console.log('Login successful', response);
        localStorage.setItem('token', response.token); 
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        if(err.status==400)
        this.toastr.error('Incorrect email or password.', 'Login failed')
        else
        console.error('Login failed', err);

      }
    });
  }
}
