import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_servises/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private auth: AuthService) { }
  model: any = {};
  @Output() cancelRegister = new EventEmitter();
  ngOnInit() {
  }

  register() {
    this.auth.register(this.model).subscribe(() => { console.log(`success`); }, (error) => { throw(error); });
  }

  cancel() {
    this.cancelRegister.emit(false);
    console.log('cancel');
  }

}
