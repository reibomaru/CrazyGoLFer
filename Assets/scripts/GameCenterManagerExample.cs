//  iOSRankingUtility.cs
//  http://kan-kikuchi.hatenablog.com/entry/iOSRankingUtility
//
//  Created by kan.kikuchi on 2016.03.31.

using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// iOSのランキング用便利クラス
/// </summary>
public static class iOSRankingUtility
{

    //=================================================================================
    //初期化
    //=================================================================================

    /// <summary>
    /// ユーザー認証
    /// </summary>
    public static void Auth(Action<bool> callBack = null)
    {
        //コールバックが設定されていない場合はログを設定
        if (callBack == null)
        {
            callBack = (success) => {
                Debug.Log(success ? "認証成功" : "認証失敗");
            };
        }
        Social.localUser.Authenticate(callBack);

        //アチーブメント獲得時の通知をONにする
        UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);

    }

    //=================================================================================
    //ランキング
    //=================================================================================

    /// <summary>
    /// リーダーボードを表示する
    /// </summary>
    public static void ShowLeaderboardUI()
    {
        Social.ShowLeaderboardUI();
    }

    /// <summary>
    /// リーダーボードにスコアを送信する
    /// </summary>
    public static void ReportScore(string leaderboardID, long score, Action<bool> callBack = null)
    {

        //コールバックが設定されていない場合はログを設定
        if (callBack == null)
        {
            callBack = (success) => {
                Debug.Log(success ? "スコア送信成功" : "スコア送信失敗");
            };
        }

        //エディター上で送信すると、エラーが出るので、送信が成功したことにする
#if UNITY_EDITOR

        callBack(true);

#else

    //送信
    Social.ReportScore (score, leaderboardID, callBack);

#endif
    }

    //=================================================================================
    //実績
    //=================================================================================

    /// <summary>
    /// 実績一覧を表示する
    /// </summary>
    public static void ShowAchievementsUI()
    {
        Social.ShowAchievementsUI();
    }

    /// <summary>
    /// 実績の進捗状況を送信する
    /// </summary>
    public static void ReportProgress(string achievementKey, float percent, Action<bool> callBack = null)
    {

        //アチーブメントのインスタンスを作成し、keyと進捗率を設定
        IAchievement achievement = Social.CreateAchievement();

        achievement.id = achievementKey;
        achievement.percentCompleted = percent;

        //コールバックが設定されていない場合はログを設定
        if (callBack == null)
        {
            callBack = (success) => {
                Debug.Log(success ? "進捗送信成功" : "進捗送信失敗");
            };
        }

        //エディター上で送信すると、エラーが出るので、送信が成功したことにする
#if UNITY_EDITOR
        callBack(true);
#else

    //送信
    achievement.ReportProgress(callBack);

#endif
    }

}