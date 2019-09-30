export const oidcSettings = {
  authority: 'https://localhost:6001',
  clientId: 'nozomi.vue',
  redirectUri: 'https://localhost:5001/auth-callback',
  responseType: 'id_token token',
  scope: 'openid profile nozomi.web.read_only'
};
