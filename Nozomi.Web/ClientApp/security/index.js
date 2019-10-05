// https://www.richard-banks.org/2018/11/securing-vue-with-identityserver-part7.html
import Oidc from 'oidc-client';

let mgr = new Oidc.UserManager({
  authority: process.env.NODE_ENV === "production" ? "https://auth.nozomi.one" : 'https://localhost:6001',
  client_id: 'nozomi.spa',
  redirect_uri: process.env.NODE_ENV === "production" ? "https://nozomi.one/auth-oidc" : 'https://localhost:5001/auth-oidc',
  response_type: 'id_token token',
  scope: 'openid profile nozomi.api',
  post_logout_redirect_uri: process.env.NODE_ENV === "production" ? "https://nozomi.one" : 'https://localhost:5001/',
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
  loadUserInfo: true
});

export default mgr;
