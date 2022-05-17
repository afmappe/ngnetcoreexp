export class Employee {
  constructor(
    public id: number,
    public apellidos: string,
    public direccion: string,
    public email: string,
    public nombres: string,
    public sueldo: number,
    public telefono: string,
    public fechaNacimiento?: string
  ) {}
}
