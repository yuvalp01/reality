export function getRole(userId: number):number {
  switch (userId) {
    case 199:
      return 1;
    case 107:
      return 2;
    default:
      return 3;
  }
}

//export const role: number;
