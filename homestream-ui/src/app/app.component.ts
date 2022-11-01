import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { appName } from 'src/main';
import { MOVIE_DESC_TEXT_LENGTH } from './common/constants';
import { Movie } from './models/movie';

@Component({
  selector: 'home-stream-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'homestream-ui';
}
