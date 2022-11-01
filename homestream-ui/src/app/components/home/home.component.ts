import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MenuAction, MOVIE_DESC_TEXT_LENGTH } from 'src/app/common/constants';
import { Menu } from 'src/app/models/menu';
import { Movie } from 'src/app/models/movie';

@Component({
  selector: 'home-stream-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  constructor(private router: Router) {}

  ngOnInit(): void {}

  menu: Menu[] = [
    {
      Text: 'folders',
      Action: MenuAction.OPEN_FOLDER,
    },
  ];

  DESC_TEXT_LENGTH = MOVIE_DESC_TEXT_LENGTH;
  sampleDes =
    'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Voluptas,exercitationem dolor quidem inventore quos soluta ratione iusto  corporis sit vero earum? Ratione ipsam explicabo vitae odit dolores sit non culpa.';

  movies: Movie[] = [
    {
      id: '101',
      title:
        'infrastructure deployment.mkvinfrastructure deployment.mkvinfrastructure deployment.mkvinfrastructure deployment.mkvinfrastructure deployment.mkvinfrastructure deployment.mkv',
      filePath: '/home/ej/Videos/infrastructure deployment.mkv',
      thumbPath:
        'https://plus.unsplash.com/premium_photo-1661396906932-2c07ee7ea835?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=687&q=80',
      gifPath: 'https://giphy.com/embed/l4lRmOLxNiNrqQgAE',
      tags: [],
    },
    {
      id: '102',
      title:
        'urban-logs.mkvurban-logs.mkvurban-logs.mkvurban-logs.mkvurban-logs.mkv',
      filePath: '/home/ej/Videos/urban-logs.mkv',
      thumbPath:
        'https://images.unsplash.com/photo-1579353977828-2a4eab540b9a?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1374&q=80',
      gifPath: 'https://giphy.com/embed/l0HlVXQ41386JrK6s',
      tags: [],
    },
    {
      id: '103',
      title: 'demo_.mkv',
      filePath: '/home/ej/Videos/demo_.mkv',
      thumbPath:
        'https://images.unsplash.com/photo-1561336313-0bd5e0b27ec8?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1470&q=80',
      gifPath: 'https://giphy.com/embed/cdScKGs0hW91Z81MER',
      tags: [],
    },
    {
      id: '104',
      title: 'infrastructure deployment.mkv',
      filePath: '/home/ej/Videos/infrastructure deployment.mkv',
      thumbPath:
        'https://images.unsplash.com/photo-1612528443702-f6741f70a049?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1480&q=80',
      gifPath: 'https://giphy.com/embed/l4lRmOLxNiNrqQgAE',
      tags: [],
    },
    {
      id: '105',
      title: 'urban-logs.mkv',
      filePath: '/home/ej/Videos/urban-logs.mkv',
      thumbPath:
        'https://images.unsplash.com/photo-1600716051809-e997e11a5d52?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1450&q=80',
      gifPath: 'https://giphy.com/embed/l0HlVXQ41386JrK6s',
      tags: [],
    },
    {
      id: '106',
      title: 'demo_.mkv',
      filePath: '/home/ej/Videos/demo_.mkv',
      thumbPath:
        'https://images.unsplash.com/photo-1588345921523-c2dcdb7f1dcd?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1470&q=80',
      gifPath: 'https://giphy.com/embed/cdScKGs0hW91Z81MER',
      tags: [],
    },
    {
      id: '107',
      title: 'infrastructure deployment.mkv',
      filePath: '/home/ej/Videos/infrastructure deployment.mkv',
      thumbPath:
        'https://images.unsplash.com/photo-1667153538104-446fa1350666?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=764&q=80',
      gifPath: 'https://giphy.com/embed/l4lRmOLxNiNrqQgAE',
      tags: [],
    },
  ];

  onMenuAction(actionId: number) {
    switch (actionId) {
      case MenuAction.OPEN_FOLDER: {
        this.router.navigate(['/folder']);
        return;
      }
    }
  }
}
