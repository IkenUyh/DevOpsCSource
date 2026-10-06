import { Controller, Get } from '@nestjs/common';
import { AppService } from './app.service';
import { Pool } from 'pg';

@Controller()
export class AppController {
  private readonly pool = new Pool({ connectionString: process.env.DATABASE_URL });

  constructor(private readonly appService: AppService) { }

  @Get()
  getHello(): string {
    return this.appService.getHello();
  }

  @Get('user')
  async getUsers(): Promise<{ source_code: string; users: string[] }> {
    const result = await this.pool.query<{ name: string }>('SELECT name FROM users');
    return {
      source_code: 'nestjs',
      users: result.rows.map((user) => user.name),
    };
  }
}
