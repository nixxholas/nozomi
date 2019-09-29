// https://www.richard-banks.org/2018/11/securing-vue-with-identityserver-part7.html
import Oidc from 'oidc-client';

let mgr = new Oidc.UserManager({
  authority: 'https://localhost:6001',
  client_id: 'nozomi.spa',
  redirect_uri: 'https://localhost:5001/callback',
  response_type: 'id_token token',
  scope: 'openid profile nozomi.api',
  post_logout_redirect_uri: 'https://localhost:5001/',
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
});

export default mgr;
