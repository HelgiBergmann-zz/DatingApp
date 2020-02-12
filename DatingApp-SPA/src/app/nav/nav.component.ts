import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_servises/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(() => console.log(`Login successefully`), error => console.log(error));
  }
  loggedIn() {
    return !!localStorage.getItem(`token`);
  }

  logOut() {
    localStorage.removeItem(`token`);
  }
}
