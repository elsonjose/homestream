import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncate'
})
export class TruncatePipe implements PipeTransform {

  transform(value: string, ...args: number[]): unknown {
    if (args[0] - 3 < 0) {
      length = 3;
    }
    if (value.length > args[0] - 3) {
      return value.slice(0, args[0] - 3) + '...';
    }
    return value;
  }

}
