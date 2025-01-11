export function getTokenFromCookie() {
    const cookieName = 'token'

    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
          cookie = cookie.trim();
          if (cookie.startsWith(cookieName + '=')) {
             return cookie.substring(cookieName.length + 1);
          }
     }
    return null;
}