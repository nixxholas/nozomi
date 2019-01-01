const path = require("path");
const webpack = require("webpack");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const CleanWebpackPlugin = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: {
        core: "./ClientApp/scripts/index.ts",
        //stream: path.resolve(__dirname, './ClientApp/scripts/stream/global.js')
    },
    output: {
        path: path.resolve(__dirname, "wwwroot"),
        filename: '[name].[contenthash].js',
        publicPath: "/"
    },
    optimization: {
        runtimeChunk: 'single',
        splitChunks: {
            chunks: 'all',
            maxInitialRequests: Infinity,
            minSize: 0
        }
    },
    resolve: {
        extensions: [".js", ".ts", ".tsx", ".scss", ".css"]
    },
    module: {
        rules: [
            // https://stackoverflow.com/questions/50536553/what-webpack4-loader-is-used-to-load-svg-files-gif-eot
            {
                test: /\.(jpg|gif)$/,
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            outputPath: 'images/',
                            name: '[name][hash].[ext]',
                        },
                    },
                ],
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader"
                }
            },
            {
                test: /\.tsx?$/,
                loader: "ts-loader"
            },
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    { loader: 'css-loader', options: { sourceMap: true, importLoaders: 1 } },
                    { loader: 'sass-loader', options: { sourceMap: true } },
                ],
            },
            // https://stackoverflow.com/questions/45489897/load-fonts-with-webpack-and-font-face
            { test: /\.(png|woff|woff2|eot|ttf|svg)$/, loader: 'url-loader?limit=100000' }
        ]
    },
    plugins: [
        new CleanWebpackPlugin(["wwwroot/*"]),
        // https://github.com/webpack-contrib/copy-webpack-plugin
        new CopyWebpackPlugin([
            { from: 'ClientApp/assets', to: 'assets' }
        ]),
        new MiniCssExtractPlugin({
            // Options similar to the same options in webpackOptions.output
            // both options are optional
            filename: "[name].css",
            chunkFilename: "[id].css"
        }),
        new webpack.ProvidePlugin({
            jQuery: 'jquery',
            $: 'jquery',
            jquery: 'jquery',
            'window.jQuery': 'jquery',
            SVGInjector : 'svg-injector',
            Typed: 'typed.js'
        }),
        new HtmlWebpackPlugin({
            hash: true,
            inject: 'head',
            // Load a custom template (lodash by default)
            template: 'ClientApp/html/_WebpackScripts.cshtml',
            filename: '../Views/Shared/_WebpackScripts.cshtml'
        })
    ]
};