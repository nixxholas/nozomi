export const oidcSettings = {
  authority: 'https://localhost:6001',
  clientId: 'nozomi.spa',
  redirectUri: 'https://localhost:5001/oidc-callback',
  responseType: 'id_token token',
  scope: 'openid profile nozomi.web.read_only'
};
